using UnityEngine;
using GameNS.Entity;

namespace GameNS.Config {
    [CreateAssetMenu(fileName = "EntityConfigBase", menuName = "ScriptableObjects/EntityConfig", order = 0)]
    public class EntityConfig : ScriptableObject {
        public string configName;
        public GameObject prefab;
        public Sprite defaultSprite;
        public EntityModification modification;
        public int sizeX = 1;
        public int sizeY = 1;
        public bool blockField = true;
        public int hardness = 10;
        public DropItemInfo[] dropItems;
        
        public Vector2 GetAreaCenter(Vector2 position) {
            var x = sizeX % 2 == 0
                ? Mathf.RoundToInt(position.x)
                : Mathf.FloorToInt(position.x) + 0.5f;
            var y = sizeY % 2 == 0
                ? Mathf.RoundToInt(position.y)
                : Mathf.FloorToInt(position.y) + 0.5f;
            return new Vector2(x, y);
        }
    }
}