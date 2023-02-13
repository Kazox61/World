using UnityEngine;

namespace SetupNS {
	[CreateAssetMenu(fileName = "TerrainSetupBase", menuName = "TerrainSetup", order = 0)]
	public class TerrainSetup : SetupBase {
		public int layer;
		public SetupTerrainModification setupTerrainModification;

		[Header("Variations")] [Range(0f, 1f)] public float defaultProbability = 1f;
		public Sprite[] variationSprites;

		[System.NonSerialized] public Tile defaultTile;

		public void OnEnable() {
			Initialize();
		}

		public void OnValidate() {
			Initialize();
		}

		private void Initialize() {
			defaultTile = CreateInstance<Tile>();
			defaultTile.sprite = defaultSprite;

			defaultTile.defaultProbability = defaultProbability;
			defaultTile.variations = variationSprites;

			if (setupTerrainModification != null) {
				setupTerrainModification.OnBegin();
			}
		}
	}
}