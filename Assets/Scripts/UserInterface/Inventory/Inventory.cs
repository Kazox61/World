using System.Collections.Generic;
using UnityEngine;

namespace UserInterfaceNS {
    public class Inventory : MonoBehaviour {
        [System.NonSerialized]
        public List<InventorySlot> slots = new();

        public void AddItem(Item item) {
            foreach (var slot in slots) {
                item = slot.AddItem(item);
                if (item.amount == 0) {
                    break;
                }
            }

            if (item.amount > 0) {
                Debug.Log($"Inventory is full, Remaining -> name: {item.itemSetup.name} with: {item.amount}" );
            } 
        }
    }
}