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
        public static ChunkManager Instance = new();
        public const int CHUNK_SIZE = 16;
        private const int ACTIVE_RADIUS = 4;

        public readonly ChunkStore chunkStore;
        public readonly List<Chunk> chunks = new();

        public ChunkManager() {
            chunkStore = new ChunkStore();
        }

        public void AddEntity(Vector2Int field, Entity entity) {
            var fieldController = GetFieldController(field);
            fieldController.entities.Add(entity);
        }

        //TODO: Rename, dont know about Enumerate
        public List<Entity> EnumerateEntities(Vector2Int field) {
            var entities = new List<Entity>();
            var chunk = GetChunkByField(field);

            if (chunk == null) {
                return entities;
            }
            
            foreach (var fieldController in chunk.fieldControllers) {
                entities.AddRange(fieldController.entities);
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

        //TODO: Move to different functions
        public void UpdateChunks(Vector2Int field) {
            var centerChunkPosition = ChunkHelper.FieldToChunkPosition(field);
            var positions = new List<Vector2Int>();
            for (int y = -ACTIVE_RADIUS; y <= ACTIVE_RADIUS; y++) {
                for (int x = -ACTIVE_RADIUS; x <= ACTIVE_RADIUS; x++) {
                    positions.Add(centerChunkPosition + new Vector2Int(x,y));
                }
            }

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
            
            foreach (var unusedChunk in unusedChunks) {
                DestructChunk(unusedChunk);
            }
            
            foreach (var position in positions) {
                ConstructChunk(position);
            }
        }

        private void ConstructChunk(Vector2Int position) {
            var success = chunkStore.TryRestoreChunk(position, out var chunk);
            if (!success) {
                chunk = Chunk.CreateChunk(position);
            }
            chunks.Add(chunk);
            
            foreach (var fieldController in chunk.fieldControllers) {
                if (fieldController.terrainGround != null) {
                    ControllerTerrainLayers.Instance.SetTile(fieldController.terrainGround, fieldController.field);
                }
                if (fieldController.terrainGrass != null) {
                    ControllerTerrainLayers.Instance.SetTile(fieldController.terrainGrass, fieldController.field);
                }
                if (fieldController.terrainDecoration != null) {
                    ControllerTerrainLayers.Instance.SetTile(fieldController.terrainDecoration, fieldController.field);
                }
            }
        }
        
        private void DestructChunk(Chunk chunk) {
            chunkStore.StoreChunk(chunk);
            
            foreach (var fieldController in chunk.fieldControllers) {
                fieldController.RemoveEntities();
            }
            ControllerTerrainLayers.Instance.ClearTilesInArea(chunk.rect);
            
            chunks.Remove(chunk);
        }
        
        // already prepared for SaveSystem to get updated ChunkData
        public DataChunk[] GetAllStoredData() {
            chunkStore.StoreChunks(chunks);
            return chunkStore.storedChunkData.Values.ToArray();
        }
    }
}