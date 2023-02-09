using UnityEngine;
using WorldNS;

namespace SetupNS {
    [CreateAssetMenu(fileName = "EntitySetupBase", menuName = "EntitySetup", order = 0)]
    public class EntitySetup : SetupBase {
        public TerrainSetup terrain;
        public ItemSetup itemSetup;
        public Vector2Int size = Vector2Int.one;
        public bool blockField = true;
        public bool[] ignoreDetectionLayers = { false, false, false };
        public int hardness = 10;
        
        public Rect GetRect(Vector2Int field) {
            return GridHelper.GetRect(field, size);
        }
    }
}