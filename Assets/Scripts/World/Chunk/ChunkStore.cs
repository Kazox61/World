using System.Collections.Generic;
using System.Linq;
using SaveSystemNS;
using ServiceNS;
using UnityEngine;

namespace WorldNS {
    public class ChunkStore {
        public readonly Dictionary<Vector2Int, DataChunk> storedChunkData = new();

        public ChunkStore() {
            var chunks= SaveSystem.Instance.World.chunks;
            foreach (var dataChunk in chunks) {
                storedChunkData.Add(new Vector2Int(dataChunk.sector.x, dataChunk.sector.y), dataChunk);
            }
        }

        public void StoreChunk(Chunk chunk) {
            var position = chunk.position;
            var exists = TryGetChunkData(position, out _);
            
            if (exists) {
                storedChunkData.Remove(position);
            }

            var storeData = chunk.chunkTransformer.ToData();
            storedChunkData.Add(position, storeData);
        }

        public void StoreChunks(List<Chunk> chunks) {
            foreach (var chunk in chunks) {
                StoreChunk(chunk);
            }
        }
        
        public bool TryRestoreChunk(Vector2Int position, out Chunk chunk) {
            var success = TryGetChunkData(position, out var dataChunk);
            chunk = null;
            if (success) {
                chunk = ObjectPool<Chunk>.Get();
                chunk.Construct();
                chunk.chunkTransformer.FromData(dataChunk);
                return true;
            }
            return false;
        }

        private bool TryGetChunkData(Vector2Int chunkPosition, out DataChunk chunkData) {
            return storedChunkData.TryGetValue(chunkPosition, out chunkData);
        }
    }
}