using UnityEngine;

namespace Extensions {
    public static class BoundExtension {
        public static bool IsInBounds(this BoundsInt bounds, Vector2 position) {
            return position.x > bounds.xMin && position.x < bounds.xMax && position.y > bounds.yMin &&
                   position.y < bounds.yMax;
        }
    }
}