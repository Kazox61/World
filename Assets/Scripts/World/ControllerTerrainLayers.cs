using Extensions;
using SetupNS;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = SetupNS.Tile;

namespace WorldNS {
	public class ControllerTerrainLayers : MonoBehaviour {
		public static ControllerTerrainLayers Instance { get; private set; }

		public Tilemap[] terrainLayers;

		private void Awake() {
			Instance = this;
		}


		public void SetTile(TerrainSetup terrainSetup, Vector2Int field) {
			var tilemap = terrainLayers[terrainSetup.layer];
			tilemap.SetTile((Vector3Int)field, terrainSetup.defaultTile);

			if (terrainSetup.setupTerrainModification == null) {
				return;
			}

			UpdateNeighborTiles(terrainSetup, field);
		}

		public Tile GetTile(Tilemap tilemap, Vector2Int field) {
			return tilemap.GetTile<Tile>((Vector3Int)field);
		}

		public void UpdateNeighborTiles(TerrainSetup terrainSetup, Vector2Int centerField) {
			var tilemap = terrainLayers[terrainSetup.layer];
			for (int y = -1; y <= 1; y++) {
				for (int x = -1; x <= 1; x++) {
					if (x != 0 || y != 0) {
						var offset = new Vector2Int(x, y);

						var tile = GetTile(tilemap, centerField + offset);
						if (tile == terrainSetup.defaultTile) {
							continue;
						}

						UpdateTile(terrainSetup, centerField + offset);
					}
				}
			}
		}

		public void UpdateTile(TerrainSetup terrainSetup, Vector2Int field) {
			var rules = TerrainHelper.GetRules();
			var ruleIndex = 0;
			var tilemap = terrainLayers[terrainSetup.layer];
			foreach (var rule in rules) {
				var matches = TerrainHelper.RuleMatches(terrainSetup, rule, field);
				if (matches) {
					tilemap.SetTile((Vector3Int)field, terrainSetup.setupTerrainModification.tiles[ruleIndex]);
					return;
				}

				ruleIndex++;
			}

			tilemap.SetTile((Vector3Int)field, null);
		}

		public void ClearTilesInArea(Rect rect) {
			var positions = rect.AllFieldsWithin();
			var tiles = new TileBase[positions.Length];
			foreach (var terrainLayer in terrainLayers) {
				terrainLayer.SetTiles(positions, tiles);
			}
		}
	}
}