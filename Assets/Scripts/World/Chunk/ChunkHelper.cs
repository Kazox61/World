using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldNS {
    public static class ChunkHelper {
        
        public static Chunk[] GetSurroundingChunks(Vector2Int centerChunkPosition, int size, ChunkManager chunkManager) {
            var surroundingChunks = new List<Chunk>();
            for (int y = -size; y <= size; y++) {
                for (int x = -size; x <= size; x++) {
                    var offset = new Vector2Int(x, y);
                    var position = offset + centerChunkPosition;
                    var chunk = chunkManager.chunks.FirstOrDefault(item => item.position.Equals(position));
                    surroundingChunks.Add(chunk);
                }
            }

            return surroundingChunks.ToArray();
        }

        public static Vector2Int[] GetSurroundingPositions(Vector2Int centerChunkPosition, int size) {
            var surroundingPositions = new List<Vector2Int>();
            for (int y = -size; y <= size; y++) {
                for (int x = -size; x <= size; x++) {
                    var offset = new Vector2Int(x, y);
                    var position = offset + centerChunkPosition;
                    surroundingPositions.Add(position);
                }
            }

            return surroundingPositions.ToArray();
        }

        public static bool IsInArea(Chunk chunk, Rect rect) {
            return rect.Overlaps(chunk.rect);
        }
        
        public static FieldController GetFieldController(Chunk chunk, Vector2Int field) {
            return chunk.fieldControllers.FirstOrDefault(item => item.field.Equals(field));
        }
        
        public static Vector2Int FieldToChunkPosition(Vector2Int field) {
            return new Vector2Int(
                field.x / ChunkManager.CHUNK_SIZE,
                field.y / ChunkManager.CHUNK_SIZE
            );
        }

    }
}