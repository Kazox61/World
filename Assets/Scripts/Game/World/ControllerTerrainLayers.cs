using GameNS;
using SetupNS;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = SetupNS.Tile;

namespace GameNS {
    public class ControllerTerrainLayers : MonoBehaviour {
        public static ControllerTerrainLayers Instance { get; private set; }

        public Tilemap[] terrainLayers;

        private void Awake() {
            Instance = this;
        }


        public void SetTile(SetupTerrain setupTerrain, Vector2Int field) {
            var terrainLayer = terrainLayers[setupTerrain.layer];

            terrainLayer.SetTile((Vector3Int)field, setupTerrain.defaultTile);

            if (setupTerrain.setupTerrainModification == null) {
                return;
            }
            UpdateNeighborTiles(setupTerrain, field);
        }

        public Tile GetTile(int layer, Vector2Int field) {
            return terrainLayers[layer].GetTile<Tile>((Vector3Int)field);
        }

        public void UpdateNeighborTiles(SetupTerrain setupTerrain, Vector2Int centerField) {
            for (int y = -1; y <= 1; y++) {
                for (int x = -1; x <= 1; x++) {
                    if (x != 0 || y != 0) {
                        var offset = new Vector2Int(x, y);

                        var tile = GetTile(setupTerrain.layer, centerField + offset);
                        if (tile == setupTerrain.defaultTile) {
                            continue;
                        }

                        UpdateTile(setupTerrain, centerField + offset);
                    }
                }
            }
        }

        public void UpdateTile(SetupTerrain setupTerrain, Vector2Int field) {
            var rules = TerrainHelper.GetRules();
            var ruleIndex = 0;
            var terrainLayer = terrainLayers[setupTerrain.layer];
            foreach (var rule in rules) {
                var matches = TerrainHelper.RuleMatches(setupTerrain, rule, field);
                if (matches) {
                    terrainLayer.SetTile((Vector3Int)field, setupTerrain.setupTerrainModification.tiles[ruleIndex]);
                    return;
                }
                ruleIndex++;
            }
            terrainLayer.SetTile((Vector3Int)field, null);
        }
    }
}