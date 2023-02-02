using System.Collections.Generic;
using UnityEngine;

namespace Extensions {
    public static class RectExtension {
        public static Vector3Int[] AllFieldsWithin(this Rect rect) {
            var fields = new List<Vector3Int>();

            for (int y = Mathf.RoundToInt(rect.yMin); y < Mathf.RoundToInt(rect.yMax); y++) {
                for (int x = Mathf.RoundToInt(rect.xMin); x < Mathf.RoundToInt(rect.xMax); x++) {
                    fields.Add(new Vector3Int(x,y));
                }
            }

            return fields.ToArray();
        }
    }
}