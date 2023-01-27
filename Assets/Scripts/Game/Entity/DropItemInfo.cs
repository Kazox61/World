using ServiceNS;
using UnityEngine;

namespace GameNS.Entity {
    [System.Serializable]
    public class DropItemInfo {
        public DropItemConfig dropItemConfig;
        public int minAmount;
        public int maxAmount;
    }
}