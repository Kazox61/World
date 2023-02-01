using UnityEngine;

namespace GameNS {
    public static class GridHelper {
        public static Rect GetRect(Vector2Int field, Vector2Int size, int extend = 0) {
            var extend2 = 2 * extend;
            var evenSizeValueCompensationX = 1 - size.x % 2;
            var evenSizeValueCompensationY = 1 - size.y % 2;

            var x = field.x - size.x / 2 + evenSizeValueCompensationX - extend;
            var y = field.y - size.y / 2 + evenSizeValueCompensationY - extend;
            
            return new Rect(x,y,size.x+extend2, size.y +extend2);
        }

        public static Vector2Int PositionToField(Vector2 position) {
            return Vector2Int.FloorToInt(position);
        }

        public static Vector2 FieldToPosition(Vector2Int field) {
            return field + Vector2.one * 0.5f;
        }
    }
}