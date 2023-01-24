using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameNS.Config {
    [CreateAssetMenu(fileName = "TerrainConfig", menuName = "ScriptableObjects/TerrainConfig", order = 0)]
    public class TerrainConfig : TileBase {
        public string configName;
        public Sprite defaultSprite;
        public Layer layer;

        public Sprite[] randomSprites;
        public Sprite[] ruleSprites;


        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.sprite = defaultSprite;
        }

        public enum Layer {
            Dirt,
            Grass,
            Floor
        }
    }
}