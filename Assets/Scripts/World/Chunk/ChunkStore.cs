using System.Collections.Generic;
using System.Linq;
using SaveSystemNS;
using UnityEngine;

namespace WorldNS {
    public class ChunkStore {
        public readonly List<DataChunk> storedChunkData;

        public ChunkStore() {
            storedChunkData = SaveSystem.Instance.World.chunks.ToList();
        }

        public void StoreChunk(Chunk chunk) {
            var chunkData = GetChunkData(chunk.position);
            
            if (chunkData.Equals(null)) {
                storedChunkData.Remove(chunkData);
            }

            var storeData = chunk.transformer.ToData();
            storedChunkData.Add(storeData);
        }

        public void StoreChunks(List<Chunk> chunks) {
            foreach (var chunk in chunks) {
                StoreChunk(chunk);
            }
        }
        
        public Chunk RestoreChunk(Vector2Int position) {
            var dataChunk = GetChunkData(position);
            if (dataChunk.fields != null) {
                return ChunkTransformer.FromData(dataChunk);
            }
            return null;
        }

        private DataChunk GetChunkData(Vector2Int chunkPosition) {
            var chunkData = storedChunkData.FirstOrDefault(item => {
                var position = new Vector2Int(item.sector.x, item.sector.y);
                return position.Equals(chunkPosition);
            });
            return chunkData;
        }
    }
}