using System.Collections.Generic;
using System.Linq;
using GameNS.Entity;
using UnityEngine;

namespace WorldNS {
    public class ChunkManager: MonoBehaviour {
        public static ChunkManager Instance { get; private set; }
        private const int CHUNK_LOAD_SIZE = 2;
        private const int UNLOAD_RADIUS = 16;
        
        private Vector2Int currentChunkPos = Vector2Int.zero;
        public List<Chunk> chunks = new List<Chunk>();

        public GameObject trackingObject;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            LoadChunks();
        }

        private void LoadChunks() {
            chunks = ChunkLoader.Load().ToList();
            
            for (var y = -CHUNK_LOAD_SIZE; y <= CHUNK_LOAD_SIZE; y++) {
                for (var x = -CHUNK_LOAD_SIZE; x <= CHUNK_LOAD_SIZE; x++) {
                    
                    var chunkPos = new Vector2Int(x,y);

                    var hasChunk = TryGetChunk(chunkPos, out var chunk);

                    if (!hasChunk) {
                        chunk = new Chunk(chunkPos);
                    }
                    StartCoroutine(chunk.Construct());

                }
            }
        }

        private void Update() {
            UpdateTouchingChunks(trackingObject.transform.position);
        }

        private void UpdateTouchingChunks(Vector3 position) {
            var center = ChunkHelper.ChunkPositionToInitialFieldPosition(currentChunkPos) + Vector2.one * (ChunkHelper.CHUNK_SIZE * 0.5f);
            var up = center.y + UNLOAD_RADIUS;
            var down = center.y - UNLOAD_RADIUS;
            var left = center.x - UNLOAD_RADIUS;
            var right = center.x + UNLOAD_RADIUS;

            if (position.y > up) {
                UpdateRelatedChunks(Vector2Int.up);
            }

            if (position.y < down) {
                UpdateRelatedChunks(Vector2Int.down);
            }

            if (position.x < left) {
                UpdateRelatedChunks(Vector2Int.left);
            }

            if (position.x > right) {
                UpdateRelatedChunks(Vector2Int.right);
            }
        }

        private void UpdateRelatedChunks(Vector2Int direction) {
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

                UpdateChunk(chunkPosUnload, chunkPosLoad);
            }
            currentChunkPos += direction;
        }

        private void UpdateChunk(Vector2Int chunkPosUnload, Vector2Int chunkPosLoad) {
            TryGetChunk(chunkPosUnload, out var unloadingChunk);

            if (unloadingChunk.coroutineDestruct != null) {
                if (unloadingChunk.coroutineConstruct != null) {
                    StopCoroutine(unloadingChunk.coroutineConstruct);
                }
            }
            else {
                unloadingChunk.coroutineDestruct = unloadingChunk.Destruct();

                StartCoroutine(unloadingChunk.coroutineDestruct);
            }
            
            var success = TryGetChunk(chunkPosLoad, out var loadingChunk);
            if (!success) {
                loadingChunk = new Chunk(chunkPosLoad);
                chunks.Add(loadingChunk);
            }

            if (loadingChunk.coroutineConstruct != null) {
                if (loadingChunk.coroutineDestruct != null) {
                    StopCoroutine(loadingChunk.coroutineDestruct);
                }
                return;
            }
            loadingChunk.coroutineConstruct = loadingChunk.Construct();
            StartCoroutine(loadingChunk.coroutineConstruct);
        }

        private bool TryGetChunk(Vector2Int chunkPosition, out Chunk chunk) {
            chunk = chunks.FirstOrDefault(element => element.chunkPosition.Equals(chunkPosition));
            return chunk != null;
        }

        public void AddEntityToChunk(Vector2Int chunkPosition, Entity entity) {
            TryGetChunk(chunkPosition, out var chunk);
            chunk.AddEntity(entity);
        }
        
        public void RemoveEntityFromChunk(Vector2Int chunkPosition, Entity entity) {
            TryGetChunk(chunkPosition, out var chunk);
            chunk.RemoveEntity(entity);
        }
        
        public void AddFieldToChunk(Vector2Int fieldPosition, Field field) {
            TryGetChunk(ChunkHelper.FieldToChunkPosition(fieldPosition), out var chunk);
            chunk.AddField(fieldPosition, field);
        }
        
        public void RemoveFieldFromChunk(Vector2Int fieldPosition) {
            TryGetChunk(ChunkHelper.FieldToChunkPosition(fieldPosition), out var chunk);
            chunk.RemoveField(fieldPosition);
        }
    }
}