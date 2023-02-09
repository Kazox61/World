using DefaultNamespace.UserInterface.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UserInterfaceNS {
    public class InventorySlot: MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        public Image itemImage;
        public TextMeshProUGUI amountText;

        public Item item;

        public delegate void Callback(PointerEventData eventData, InventorySlot inventorySlot);
        public Callback callback;

        private void OnEnable() {
            callback += (data, slot) => {
            };
        }

        private void OnDisable() {
            
        }

        public void OnPointerClick(PointerEventData eventData) {
            ControllerInventory.instance.activeSlot = this;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            callback?.Invoke(eventData, this);
        }

        public void OnPointerExit(PointerEventData eventData) {
            callback?.Invoke(eventData, this);
        }

        public Item AddItem(Item addingItem) {
            if (!CanAddItem(addingItem)) {
                return addingItem;
            }
            
            if (item == null) {
                var hasSplit = addingItem.TrySplitOverhead(out var overhead);
                item = addingItem;
                UpdateSlot();
                if (!hasSplit) {
                    overhead = new Item(addingItem.itemSetup, 0);
                }
                return overhead;
            }
            
            var remainingAmount = item.Add(addingItem.amount);
            addingItem.amount = remainingAmount;
            UpdateSlot();
            return addingItem;
        }
        
        private bool CanAddItem(Item addingItem) {
            if (item == null) {
                return true;
            }
            if (addingItem.IsEqual(item)) {
                return !item.IsFull();
            }

            return false;

        }

        private void UpdateSlot() {
            itemImage.sprite = item.itemSetup.sprite;
            amountText.text = item.amount.ToString();
        }
    }
}