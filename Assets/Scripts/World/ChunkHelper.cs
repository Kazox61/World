using UnityEngine;

namespace WorldNS {
    public static class ChunkHelper {
        public const int CHUNK_SIZE = 16;
        
        public static Vector2Int ChunkPositionToInitialFieldPosition(Vector2Int chunkPos) {
            return chunkPos * CHUNK_SIZE;
        }

        public static Vector2Int FieldToChunkPosition(Vector2Int position) {
            return new Vector2Int(
                Mathf.FloorToInt((float)position.x / CHUNK_SIZE),
                Mathf.FloorToInt((float)position.y / CHUNK_SIZE)
            );
        }

        public static Vector2Int FieldArrayIndexToPosition(int index, Vector2Int initialFieldPosition) {
            return new Vector2Int(
                index % CHUNK_SIZE + initialFieldPosition.x,
                Mathf.FloorToInt((float)index / CHUNK_SIZE) + initialFieldPosition.y
            );
        }

        public static Vector2Int PositionToFieldPosition(Vector2 position) {
            return new Vector2Int(
                Mathf.FloorToInt(position.x),
                Mathf.FloorToInt(position.y)
            );
        }
    }
}