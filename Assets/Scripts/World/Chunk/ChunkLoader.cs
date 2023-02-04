using UnityEngine;

namespace WorldNS {
    public class ChunkLoader {
        private readonly ChunkStore chunkStore;
        private readonly ChunkManager chunkManager;

        public ChunkLoader(ChunkManager manager,ChunkStore store) {
            chunkStore = store;
            chunkManager = manager;
        }
        
        public Chunk ConstructChunk(Vector2Int position) {
            var success = chunkStore.TryRestoreChunk(position, out var chunk);
            if (!success) {
                chunk = Chunk.CreateChunk(position);
            }
            
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

            return chunk;
        }
        
        public void DestructChunk(Chunk chunk) {
            chunkStore.StoreChunk(chunk);
            
            foreach (var fieldController in chunk.fieldControllers) {
                fieldController.RemoveEntities();
            }
            ControllerTerrainLayers.Instance.ClearTilesInArea(chunk.rect);
            
            chunkManager.chunks.Remove(chunk);
        }

    }
}