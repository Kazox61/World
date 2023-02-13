using SetupNS;

namespace UserInterfaceNS {
	public class Item {
		public ItemSetup itemSetup;
		public int amount;

		public Item() { }

		public Item(ItemSetup itemSetup, int amount = 1) {
			this.itemSetup = itemSetup;
			this.amount = amount;
		}

		public bool IsEqual(Item other) {
			return itemSetup == other.itemSetup;
		}

		public bool IsFull() {
			return amount >= itemSetup.stackSize;
		}

		public bool IsStackable() {
			return itemSetup.stackSize > 1;
		}

		public int Add(int addingAmount) {
			var possibleAmount = amount + addingAmount;

			if (possibleAmount > itemSetup.stackSize) {
				var remainingAmount = possibleAmount - itemSetup.stackSize;
				return remainingAmount;
			}

			amount = possibleAmount;
			return 0;
		}

		public bool TrySplitOverhead(out Item overhead) {
			overhead = null;
			if (amount <= itemSetup.stackSize) {
				return false;
			}

			var remaining = amount - itemSetup.stackSize;
			overhead = new Item(itemSetup, remaining);

			amount = itemSetup.stackSize;

			return true;
		}
	}
}