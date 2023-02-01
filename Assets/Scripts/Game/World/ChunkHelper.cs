using UnityEngine;

namespace GameNS {
    public static class ChunkHelper {
        public const int CHUNK_SIZE = 16;
        public const int CONSTRUCT_AMOUNT_PER_FRAME = 10;
        public const int DESTRUCT_AMOUNT_PER_FRAME = 10;
        
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

        public static int PositionToFieldArrayIndex(Vector2Int fieldPosition, Vector2Int chunkPosition) {
            var pos = fieldPosition - ChunkHelper.ChunkPositionToInitialFieldPosition(chunkPosition);
            return pos.x + pos.y * ChunkHelper.CHUNK_SIZE;
        }
    }
}