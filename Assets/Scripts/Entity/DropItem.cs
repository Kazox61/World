using UnityEngine;

namespace GameNS.Entity {
    [CreateAssetMenu(fileName = "DropItem", menuName = "ScriptableObjects/DropItem", order = 0)]
    public class DropItem : ScriptableObject {
        public string itemName;
        public int minAmount;
        public int maxAmount;
    }
}