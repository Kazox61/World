using UnityEngine;

namespace SetupNS {
	[CreateAssetMenu(fileName = "ItemSetup", menuName = "ItemSetup", order = 0)]
	public class ItemSetup : ScriptableObject {
		public Sprite sprite;
		public int stackSize;
		public PlaceAction placeAction;
		public ConsumeAction consumeAction;
		public ToolAction toolAction;
		public AttackAction attackAction;
	}
}