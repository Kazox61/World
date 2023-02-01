using UnityEngine;
using UnityEngine.Tilemaps;

namespace SetupNS {
    public class Tile : TileBase {
        public Sprite sprite;
        public float defaultProbability = 1f;
        public Sprite[] variations;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            if (defaultProbability >= 0.99f) {
                tileData.sprite = sprite;
                return;
            }

            var random = Random.value;

            if (random < defaultProbability) {
                tileData.sprite = sprite;
                return;
            }

            var variationIndex = Random.Range(0, variations.Length);
            tileData.sprite = variations[variationIndex];
        }
    }
}