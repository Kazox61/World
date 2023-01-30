using UnityEngine;
using UnityEngine.Tilemaps;

namespace SetupNS {
    public class Tile : TileBase {
        public Sprite sprite;
        public RuleTile tileBase;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
            tileData.sprite = sprite;
        }
    }
}