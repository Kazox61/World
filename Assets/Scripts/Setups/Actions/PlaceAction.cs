using UnityEngine;

namespace SetupNS {
	[CreateAssetMenu(fileName = "PlaceAction", menuName = "Action/PlaceActionSetup", order = 0)]
	public class PlaceAction : Action {
		public EntitySetup entity;
	}
}