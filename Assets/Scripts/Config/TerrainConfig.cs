using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameNS.Config {
    [CreateAssetMenu(fileName = "TerrainConfig", menuName = "ScriptableObjects/TerrainConfig", order = 0)]
    public class TerrainConfig : TileBase {
        public string configName;
        public Sprite defaultSprite;
        public Layer layer;
        [Range(0f,1f)]
        public float defaultProbability = 1f;
        public Sprite[] variations;
        public Sprite[] ruleSprites;


        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            if (defaultProbability >= 1f) {
                tileData.sprite = defaultSprite;
                return;
            }
            var random = Random.value;
            
            if (random < defaultProbability) {
                tileData.sprite = defaultSprite;
                return;
            }

            var variationIndex = Random.Range(0, variations.Length);
            tileData.sprite = variations[variationIndex];
        }

        public enum Layer {
            Water,
            Dirt,
            Grass,
            Decoration
        }
    }

    [System.Serializable]
    public class Variation {
        public float defaultProbability = 1f;
        public Sprite[] variations;
    }
}