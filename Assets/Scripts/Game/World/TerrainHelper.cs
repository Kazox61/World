using SetupNS;
using UnityEngine;
using GameNS;

namespace GameNS {
    public static class TerrainHelper {
        public static TerrainRule[] GetRules() {
            return new TerrainRule[] {
                new() { input = new[] { 0, 1, 0, 2, 1, 2, 2, 0 }, output = 0 },
                new() { input = new[] { 0, 1, 0, 1, 2, 0, 2, 2 }, output = 1 },
                new() { input = new[] { 2, 2, 1, 2, 2, 2, 2, 2 }, output = 2 },
                new() { input = new[] { 1, 2, 2, 2, 2, 2, 2, 2 }, output = 3 },
                new() { input = new[] { 2, 2, 0, 2, 1, 2, 2, 0 }, output = 4 },
                new() { input = new[] { 0, 2, 2, 1, 2, 0, 2, 2 }, output = 5 },
                
                new() { input = new[] { 2, 2, 0, 2, 1, 0, 1, 0 }, output = 6 },
                new() { input = new[] { 0, 2, 2, 1, 2, 0, 1, 0 }, output = 7 },
                new() { input = new[] { 2, 2, 2, 2, 2, 2, 2, 1 }, output = 8 },
                new() { input = new[] { 2, 2, 2, 2, 2, 1, 2, 2 }, output = 9 },
                new() { input = new[] { 2, 2, 2, 2, 2, 0, 1, 0 }, output = 10 },
                new() { input = new[] { 0, 1, 0, 2, 2, 2, 2, 2 }, output = 11 },
                
                new() { input = new[] { 0, 1, 0, 2, 2, 1, 2, 1 }, output = 12 },
                new() { input = new[] { 0, 2, 1, 1, 2, 0, 2, 1 }, output = 13 },
                new() { input = new[] { 0, 2, 0, 1, 1, 0, 2, 0 }, output = 14 },
                new() { input = new[] { 0, 1, 0, 2, 2, 0, 1, 0 }, output = 15 },
                new() { input = new[] { 0, 2, 0, 1, 1, 0, 1, 0 }, output = 16 },
                new() { input = new[] { 0, 1, 0, 2, 1, 0, 1, 0 }, output = 17 },
                
                new() { input = new[] { 1, 2, 0, 2, 1, 1, 2, 0 }, output = 18 },
                new() { input = new[] { 1, 2, 1, 2, 2, 0, 1, 0 }, output = 19 },
                new() { input = new[] { 0, 1, 0, 1, 1, 0, 1, 0 }, output = 20 },
                new() { input = new[] { 1, 2, 1, 2, 2, 1, 2, 1 }, output = 21 },
                new() { input = new[] { 0, 1, 0, 1, 1, 0, 2, 0 }, output = 22 },
                new() { input = new[] { 0, 1, 0, 1, 2, 0, 1, 0 }, output = 23 },
                
                new() { input = new[] { 0, 1, 0, 2, 1, 1, 2, 0 }, output = 24 },
                new() { input = new[] { 0, 1, 0, 1, 2, 0, 2, 1 }, output = 25 },
                new() { input = new[] { 1, 2, 1, 2, 2, 2, 2, 1 }, output = 26 },
                new() { input = new[] { 1, 2, 1, 2, 2, 1, 2, 2 }, output = 27 },
                new() { input = new[] { 2, 2, 2, 2, 2, 1, 2, 1 }, output = 28 },
                new() { input = new[] { 2, 2, 1, 2, 2, 2, 2, 1 }, output = 29 },
                
                new() { input = new[] { 1, 2, 0, 2, 1, 0, 1, 0 }, output = 30 },
                new() { input = new[] { 0, 2, 1, 1, 2, 0, 1, 0 }, output = 31 },
                new() { input = new[] { 2, 2, 1, 2, 2, 1, 2, 1 }, output = 32 },
                new() { input = new[] { 1, 2, 2, 2, 2, 1, 2, 1 }, output = 33 },
                new() { input = new[] { 1, 2, 2, 2, 2, 1, 2, 2 }, output = 34 },
                new() { input = new[] { 1, 2, 1, 2, 2, 2, 2, 2 }, output = 35 },
                
                new() { input = new[] { 0, 1, 0, 2, 2, 2, 2, 1 }, output = 36 },
                new() { input = new[] { 0, 1, 0, 2, 2, 1, 2, 2 }, output = 37 },
                new() { input = new[] { 1, 2, 0, 2, 1, 2, 2, 0 }, output = 38 },
                new() { input = new[] { 0, 2, 1, 1, 2, 0, 2, 2 }, output = 39 },
                new() { input = new[] { 2, 2, 1, 2, 2, 1, 2, 2 }, output = 40 },
                new() { input = new[] { 1, 2, 2, 2, 2, 2, 2, 1 }, output = 41 },
                
                new() { input = new[] { 2, 2, 1, 2, 2, 0, 1, 0 }, output = 42 },
                new() { input = new[] { 1, 2, 2, 2, 2, 0, 1, 0 }, output = 43 },
                new() { input = new[] { 2, 2, 0, 2, 1, 1, 2, 0 }, output = 44 },
                new() { input = new[] { 0, 2, 2, 1, 2, 0, 2, 1 }, output = 45 },
            };
        }

        public static bool RuleMatches(SetupTerrain terrainSetup, TerrainRule rule, Vector2Int centerField) {
            var index = 0;
            for (int y = -1; y <= 1; y++) {
                for (int x = -1; x <= 1; x++) {
                    if (x != 0 || y != 0) {
                        var offset = new Vector2Int(x, y);
                        var field = centerField + offset;
                        var tile = ControllerTerrainLayers.Instance.GetTile(terrainSetup.layer, field);
                        
                        if ((rule.input[index] == 1 && tile != terrainSetup.defaultTile) || (rule.input[index] == 2 && tile == terrainSetup.defaultTile)) {
                            return false;
                        }

                        index++;
                    }
                }
				
            }
            return true;
        }
    }
}