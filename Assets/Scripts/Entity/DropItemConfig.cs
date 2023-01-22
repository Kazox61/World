using UnityEngine;

namespace GameNS.Entity {
    [CreateAssetMenu(fileName = "DropItem", menuName = "ScriptableObjects/DropItem", order = 0)]
    public class DropItemConfig : ScriptableObject {
        public string itemName;
        public Sprite sprite;
    }
}