using GameNS;
using UnityEngine;

namespace SetupNS {
    [CreateAssetMenu(fileName = "SetupEntityBase", menuName = "SetupEntity", order = 0)]
    public class SetupEntity : SetupBase {
        public Sprite defaultSprite;
        public Vector2Int size = Vector2Int.one;
        public EntityModification modification;
        public bool blockField = true;
        public int hardness = 10;
        
        public Vector2 GetAreaCenter(Vector2 position) {
            var x = size.x % 2 == 0
                ? Mathf.RoundToInt(position.x)
                : Mathf.FloorToInt(position.x) + 0.5f;
            var y = size.y % 2 == 0
                ? Mathf.RoundToInt(position.y)
                : Mathf.FloorToInt(position.y) + 0.5f;
            return new Vector2(x, y);
        }

        public Rect GetRect(Vector2Int field) {
            return GridHelper.GetRect(field, size);
        }
    }
}