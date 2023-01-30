using UnityEngine;

namespace GameNS {
    public static class GridHelper {
        public static Rect GetRect(Vector2Int field, Vector2Int size) {
            return new Rect(field, size);
        }
    }
}