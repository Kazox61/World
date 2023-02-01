using System;
using UnityEngine;
using WorldNS;

namespace SetupNS {
    [CreateAssetMenu(fileName = "SetupTerrainBase", menuName = "SetupTerrain", order = 0)]
    public class SetupTerrain : SetupBase {
        public Sprite defaultSprite;
        public int layer;
        public bool[] ignoreDetectionLayers = { false, false, true };
        public SetupTerrainModification setupTerrainModification;
        
        [Header("Variations")]
        [Range(0f, 1f)] 
        public float defaultProbability = 1f;
        public Sprite[] variationSprites;
        
        [System.NonSerialized]
        public Tile defaultTile;
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


        public static void CreateTerrain(SetupTerrain setupTerrain, Vector2Int field) {
            if (!CanCreateTerrain(setupTerrain, field)) {
                return;
            }
            ControllerTerrainLayers.Instance.SetTile(setupTerrain, field);
        }

        public static bool CanCreateTerrain(SetupTerrain setupTerrain, Vector2Int field) {
            var detectionSet = new DetectionSet() { field = field, setupTerrain =  setupTerrain};
            var areaDetection = AreaDetectionBuilder.Instance.GetAreaDetection(setupTerrain.ignoreDetectionLayers);
            return areaDetection.IsClean(detectionSet);
        }
    }
}