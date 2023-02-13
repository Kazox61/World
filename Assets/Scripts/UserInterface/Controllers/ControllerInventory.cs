using SetupNS;
using UnityEngine;
using UserInterfaceNS;

namespace WorldNS.UserInterface.Controllers {
	public class ControllerInventory : MonoBehaviour {
		public static ControllerInventory instance;
		public RectTransform imageIndicator;
		public Inventory inventory;
		public InventorySlot inventorySlotPrefab;
		public Transform containerContent;

		[System.NonSerialized] public InventorySlot activeSlot;

		private void Awake() {
			instance = this;
		}

		private void Start() {
			ImportAllItems();
			activeSlot = inventory.slots[0];
			UpdateIndicator();
		}

		public void Update() {
			UpdateIndicator();
		}

		private void UpdateIndicator() {
			imageIndicator.transform.position = activeSlot.transform.position;
		}


		private void ImportAllItems() {
			var setups = Resources.LoadAll<ItemSetup>("Items");
			foreach (var itemSetup in setups) {
				var slot = Instantiate(inventorySlotPrefab, containerContent);
				inventory.slots.Add(slot);
				inventory.AddItem(new Item(itemSetup));
			}
		}
	}
}