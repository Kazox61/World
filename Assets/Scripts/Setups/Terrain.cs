using ServiceNS;
using Unity.VisualScripting;
using UnityEngine;
using WorldNS;

namespace SetupNS {
    public class Terrain: PoolableObject {
        public TerrainSetup terrainSetup;
        private Vector2Int field;
        

        public void Initialize(TerrainSetup terrainSetup, Vector2Int field) {
            this.terrainSetup = terrainSetup;
            this.field = field;
        }

        public static Terrain CreateTerrain(TerrainSetup terrainSetup, FieldController fieldController) {
            if (terrainSetup == null) {
                return null;
            } 
            var terrain = ObjectPool<Terrain>.Get();
            terrain.Initialize(terrainSetup, fieldController.field);

            ControllerTerrainLayers.Instance.SetTile(terrainSetup, terrain.field);
            return terrain;
        }

        public static Terrain PlaceTerrain(TerrainSetup terrainSetup, Vector2Int field) {
            if (!CanCreateTerrain(terrainSetup, field)) {
                return null;
            }

            var fieldController = ChunkManager.Instance.GetFieldController(field);
            var terrain = CreateTerrain(terrainSetup, fieldController);

            if (terrain == null) {
                return null;
            }
            
            if (terrainSetup.layer == 0) {
                fieldController.ground = terrain;
            }
            
            if (terrainSetup.layer == 1) {
                fieldController.grass = terrain;
            }
            if (terrainSetup.layer == 4) {
                fieldController.decoration = terrain;
            }
            return terrain;
        }
        

        public static bool CanCreateTerrain(TerrainSetup terrainSetup, Vector2Int field) {
            var detectionSet = new DetectionSet() { field = field, terrainSetup =  terrainSetup};
            var areaDetection = AreaDetectionBuilder.Instance.GetAreaDetection(terrainSetup.ignoreDetectionLayers);
            return areaDetection.IsClean(detectionSet);
        }
    }
}