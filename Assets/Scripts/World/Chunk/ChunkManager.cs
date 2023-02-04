using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameNS;
using Newtonsoft.Json;
using SaveSystemNS;
using SetupNS;
using UnityEngine;

namespace WorldNS {
    public class ChunkManager {
        private static ChunkManager instance;

        public static ChunkManager Instance => instance ??= new ChunkManager();
        public const int CHUNK_SIZE = 16;
        private const int ACTIVE_RADIUS = 4;

        private readonly ChunkStore chunkStore;
        private readonly ChunkLoader chunkLoader;
        public readonly List<Chunk> chunks = new();

        private ChunkManager() {
            chunkStore = new ChunkStore();
            chunkLoader = new ChunkLoader(this, chunkStore);
        }

        public void AddEntity(Vector2Int field, Entity entity) {
            var fieldController = GetFieldController(field);
            fieldController.entities.Add(entity);
        }

        //Get All Entities in 3x3 Chunks, field is in center Chunk
        public List<Entity> EnumerateEntities(Vector2Int field) {
            var entities = new List<Entity>();

            var chunksInArea = ChunkHelper.GetChunksInArea(field, 1, this);
            
            foreach (var chunk in chunksInArea) {
                if (chunk == null) {
                    continue;
                }
                foreach (var fieldController in chunk.fieldControllers) {
                    entities.AddRange(fieldController.entities);
                }
                
            }
            return entities;
        }
        
        public FieldController GetFieldController(Vector2Int field) {
            var chunk = GetChunkByField(field);
            return ChunkHelper.GetFieldController(chunk, field);
        }

        public Chunk GetChunkByField(Vector2Int field) {
            var fieldRect = new Rect(field.x, field.y, 1, 1);

            foreach (var chunk in chunks) {
                if (chunk.rect.Overlaps(fieldRect)) {
                    return chunk;
                }
            }

            return null;
        }

        public void UpdateChunks(Vector2Int field) {
            var positions = GetActiveChunkPositions(field);

            var unusedChunks = KeepChunksAlive(ref positions);
            foreach (var unusedChunk in unusedChunks) {
                chunkLoader.DestructChunk(unusedChunk);
            }
            
            foreach (var position in positions) {
                var chunk = chunkLoader.ConstructChunk(position);
                chunks.Add(chunk);
            }
        }

        private List<Chunk> KeepChunksAlive(ref List<Vector2Int> positions) {
            var unusedChunks = new List<Chunk>();
            foreach (var chunk in chunks) {
                if (positions.Contains(chunk.position)) {
                    positions.Remove(chunk.position);

                    chunk.Keepalive();
                    continue;
                }

                var isUnused = chunk.UpdateKeepalive();
                if (isUnused) {
                    unusedChunks.Add(chunk);
                }
            }

            return unusedChunks;
        }

        private List<Vector2Int> GetActiveChunkPositions(Vector2Int field) {
            var centerChunkPosition = ChunkHelper.FieldToChunkPosition(field);
            var positions = new List<Vector2Int>();
            for (int y = -ACTIVE_RADIUS; y <= ACTIVE_RADIUS; y++) {
                for (int x = -ACTIVE_RADIUS; x <= ACTIVE_RADIUS; x++) {
                    positions.Add(centerChunkPosition + new Vector2Int(x, y));
                }
            }

            return positions;
        }


        // already prepared for SaveSystem to get updated ChunkData
        public DataChunk[] GetAllStoredData() {
            chunkStore.StoreChunks(chunks);
            return chunkStore.storedChunkData.Values.ToArray();
        }
    }
}