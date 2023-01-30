using UnityEngine;

namespace Extensions {
    public static class RectExtension {
        public static bool OverlapsWith(this Rect self, Rect other) {
            return self.xMin < other.xMax &&
                    self.xMax > other.xMin &&
                    self.yMin < other.yMax &&
                    self.yMax > other.yMin;
        }
    }
}