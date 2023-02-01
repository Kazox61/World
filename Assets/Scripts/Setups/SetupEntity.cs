using GameNS;
using UnityEngine;

namespace SetupNS {
    [CreateAssetMenu(fileName = "SetupEntityBase", menuName = "SetupEntity", order = 0)]
    public class SetupEntity : SetupBase {
        public Sprite defaultSprite;
        public Vector2Int size = Vector2Int.one;
        public bool blockField = true;
        public bool[] ignoreDetectionLayers = { false, false, false };
        public int hardness = 10;
        
        public Rect GetRect(Vector2Int field) {
            return GridHelper.GetRect(field, size);
        }
    }
}