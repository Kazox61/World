using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameNS.Config;
using GameNS.Entity;
using GameNS.WorldEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldNS {
    public class Environment : MonoBehaviour {
        public const int CHUNK_SIZE = 16;
        private const int UNLOAD_RADIUS = 16;
        private const int CHUNK_LOAD_SIZE = 2;
        
        public Tilemap[] layers;
        public GameObject gameObjectFollowing;
        
        private readonly Dictionary<Vector2Int, Field> fields = new();
        private List<Chunk> chunks = new List<Chunk>();
        private Dictionary<Vector2Int, IEnumerator> loadingChunks = new Dictionary<Vector2Int, IEnumerator>();
        private Dictionary<Vector2Int, IEnumerator> unloadingChunks = new Dictionary<Vector2Int, IEnumerator>();
        private Dictionary<Vector2Int, IEnumerator> loadingQueueChunks = new Dictionary<Vector2Int, IEnumerator>();
        private Dictionary<Vector2Int, IEnumerator> unloadingQueueChunks = new Dictionary<Vector2Int, IEnumerator>();
        private Vector2Int currentChunkPos;

        public void Start() {
            chunks = ChunkLoader.Load().ToList();
            for (var y = -CHUNK_LOAD_SIZE; y <= CHUNK_LOAD_SIZE; y++) {
                for (var x = -CHUNK_LOAD_SIZE; x <= CHUNK_LOAD_SIZE; x++) {
                    
                    var chunkPos = new Vector2Int(x,y);
                    
                    var loadContainer = new CoroutineContainer();
                    var load = LoadChunkAsync(chunkPos, loadContainer);
                    loadContainer.enumerator = load;
                    
                    StartCoroutine(load);
                }
            }

            InputController.Instance.OnKeyDownKeyboardP += Save;

        }
        
        private void Update() {
            ProcessLeaveChunk(gameObjectFollowing.transform.position);
        }
        
        private void UpdateChunk(Chunk chunk, Action<Vector2Int> chunkFieldCallback = null, Action<Entity> chunkEntityCallback = null) {
            foreach (Vector2Int position in chunk.bounds.allPositionsWithin) {
                fields.TryGetValue(position, out var field);
                chunkFieldCallback?.Invoke(position);
                var fieldIndex = (position.x - chunk.chunkPos.x * CHUNK_SIZE) + (position.y - chunk.chunkPos.y * CHUNK_SIZE) * CHUNK_SIZE;
                chunk.fields[fieldIndex] = field;
            }
            
            var entitiesData = new List<EntityData>();
            var copy = new Entity[FieldController.Instance.entities.Count];
            FieldController.Instance.entities.CopyTo(copy);
            foreach (var entity in copy) {
                if (!chunk.bounds.IsInBounds(entity.transform.position)) continue;
                var entityData = new EntityData(entity);
                entitiesData.Add(entityData);
                chunkEntityCallback?.Invoke(entity);
            }
            chunk.entities = entitiesData.ToArray();
        }

        public static Vector2Int GetRefPoint(Vector2Int chunkPos) {
            return chunkPos * CHUNK_SIZE;
        }

        private IEnumerator LoadChunkAsync(Vector2Int chunkPos, CoroutineContainer container) {
            var self = container.enumerator;
            if (loadingChunks.ContainsKey(chunkPos)) {
                StopCoroutine(self);
                if (unloadingQueueChunks.TryGetValue(chunkPos, out var other)) {
                    StopCoroutine(other);
                }
            }
            
            loadingQueueChunks.Add(chunkPos, self);
            while (unloadingChunks.ContainsKey(chunkPos)) {
                yield return null;
            }
            
            loadingQueueChunks.Remove(chunkPos);
            loadingChunks.Add(chunkPos, self);
            
            var chunk = chunks.FirstOrDefault(match => match.chunkPos.Equals(chunkPos));
            if (chunk == null) {
                chunk = new Chunk(chunkPos);
                chunks.Add(chunk);
            }
            
            for (var i = 0; i < chunk.fields.Length; i++) {
                var field = chunk.fields[i];
                var x = i % CHUNK_SIZE + chunk.bounds.xMin;
                var y = Mathf.FloorToInt(i / CHUNK_SIZE) + chunk.bounds.yMin;

                var terrainConfig = TerrainConfigCore.GetConfig(string.IsNullOrEmpty(field.terrain) ? "Dirt" : field.terrain);
                field.terrain = terrainConfig.configName;
                layers[(int)terrainConfig.layer].SetTile(new Vector3Int(x,y,0), terrainConfig);
                fields.Add(new Vector2Int(x, y), field);
                yield return null;
            }

            foreach (var entityData in chunk.entities) {
                FieldController.Instance.TryCreateEntity(
                    EntityConfigCore.GetConfig(entityData.name),
                    new Vector2(entityData.position.x, entityData.position.y),
                    out _);
                yield return null;
            }

            loadingChunks.Remove(chunkPos);
        }
        
        private IEnumerator UnloadChunkAsync(Vector2Int chunkPos, CoroutineContainer container) {
            var self = container.enumerator;
            if (unloadingChunks.ContainsKey(chunkPos)) {
                StopCoroutine(self);
                if (loadingQueueChunks.TryGetValue(chunkPos, out var other)) {
                    StopCoroutine(other);
                }
            }
            
            unloadingQueueChunks.Add(chunkPos, self);
            while (loadingChunks.ContainsKey(chunkPos)) {
                yield return null;
            }
            
            unloadingQueueChunks.Remove(chunkPos);
            unloadingChunks.Add(chunkPos, self);
            
            var chunk = new Chunk(chunkPos);
            
            UpdateChunk(chunk, pos => {
                layers[0].SetTile((Vector3Int)pos, null);
                fields.Remove(pos);
            }, entity => {
                FieldController.Instance.RemoveEntity(entity);
            });
            yield return null;
            var index = chunks.FindIndex(match => match.chunkPos.Equals(chunkPos));
            chunks[index] = chunk;

            unloadingChunks.Remove(chunkPos);
        }

        private void ProcessLeaveChunk(Vector3 position) {
            var center = GetRefPoint(currentChunkPos) + Vector2.one * (CHUNK_SIZE * 0.5f);
            var up = center.y + UNLOAD_RADIUS;
            var down = center.y - UNLOAD_RADIUS;
            var left = center.x - UNLOAD_RADIUS;
            var right = center.x + UNLOAD_RADIUS;

            if (position.y > up) {
                OnLeaveChunk(Vector2Int.up);
            }

            if (position.y < down) {
                OnLeaveChunk(Vector2Int.down);
            }

            if (position.x < left) {
                OnLeaveChunk(Vector2Int.left);
            }

            if (position.x > right) {
                OnLeaveChunk(Vector2Int.right);
            }
        }

        private void OnLeaveChunk(Vector2Int direction) {
            var reverse = direction * -CHUNK_LOAD_SIZE;
            var doubled = direction * (CHUNK_LOAD_SIZE+1);
            for (int i = -CHUNK_LOAD_SIZE; i <= CHUNK_LOAD_SIZE; i++) {
                var offsetUnload = new Vector2Int(reverse.x, i);
                var offsetLoad = new Vector2Int(doubled.x, i);
                if (direction.x == 0) {
                    offsetUnload = new Vector2Int(i, reverse.y);
                    offsetLoad = new Vector2Int(i, doubled.y);
                }
                var chunkPosUnload = currentChunkPos + offsetUnload;
                var chunkPosLoad = currentChunkPos + offsetLoad;


                var unloadContainer = new CoroutineContainer();
                var loadContainer = new CoroutineContainer();
                var unload = UnloadChunkAsync(chunkPosUnload, unloadContainer);
                var load = LoadChunkAsync(chunkPosLoad, loadContainer);
                unloadContainer.enumerator = unload;
                loadContainer.enumerator = load;
                
                
                StartCoroutine(unload);
                StartCoroutine(load);
            }
            currentChunkPos += direction;
        }

        private void Save() {
            foreach (var chunk in chunks) {
                UpdateChunk(chunk);
            }
            ChunkLoader.Save(chunks);
        }
    }

    public class CoroutineContainer {
        public IEnumerator enumerator;
    }
}