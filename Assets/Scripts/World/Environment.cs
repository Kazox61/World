using System;
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
        private const int UNLOAD_RADIUS = 10;
        private const int CHUNK_LOAD_SIZE = 1;
        
        public Tilemap[] layers;
        public GameObject gameObjectFollowing;
        
        private readonly Dictionary<Vector2Int, Field> fields = new();
        private List<Chunk> chunks = new List<Chunk>();
        public Vector2Int currentChunkPos;

        public void Start() {
            chunks = ChunkLoader.Load().ToList();
            for (var y = -CHUNK_LOAD_SIZE; y <= CHUNK_LOAD_SIZE; y++) {
                for (var x = -CHUNK_LOAD_SIZE; x <= CHUNK_LOAD_SIZE; x++) {
                    LoadChunk(new Vector2Int(x,y));
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

        private void LoadChunk(Vector2Int chunkPos) {
            var chunk = chunks.FirstOrDefault(match => match.chunkPos.Equals(chunkPos));
            if (chunk == null) {
                chunk = new Chunk(chunkPos);
                chunks.Add(chunk);
            }
            
            for (var i = 0; i < chunk.fields.Length; i++) {
                var field = chunk.fields[i];
                var x = i % CHUNK_SIZE + chunk.bounds.xMin;
                var y = Mathf.FloorToInt(i / CHUNK_SIZE) + chunk.bounds.yMin;

                var dirt = TerrainConfigCore.GetConfig("Dirt");
                layers[0].SetTile(new Vector3Int(x,y,0), dirt);
                fields.Add(new Vector2Int(x, y), field);
            }

            foreach (var entityData in chunk.entities) {
                FieldController.Instance.TryCreateEntity(
                    EntityConfigCore.GetConfig(entityData.name),
                    new Vector2(entityData.position.x, entityData.position.y),
                    out _);
            }
        }

        private void UnLoadChunk(Vector2Int chunkPos) {
            var chunk = new Chunk(chunkPos);
            
            UpdateChunk(chunk, pos => {
                layers[0].SetTile((Vector3Int)pos, null);
                fields.Remove(pos);
            }, entity => {
                FieldController.Instance.RemoveEntity(entity);
            });
            
            var index = chunks.FindIndex(match => match.chunkPos.Equals(chunkPos));
            chunks[index] = chunk;
        }

        public static Vector2Int GetRefPoint(Vector2Int chunkPos) {
            return chunkPos * CHUNK_SIZE;
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
                UnLoadChunk(chunkPosUnload);
                LoadChunk(chunkPosLoad);
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
}