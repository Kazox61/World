using UnityEngine;

namespace SetupNS {
    [CreateAssetMenu(fileName = "SetupTerrainBase", menuName = "SetupTerrain", order = 0)]
    public class SetupTerrain : SetupBase {
        public Sprite defaultSprite;
        public Sprite[] ruleSprites;
        
        [System.NonSerialized]
        public Tile[] tiles;
        
        [System.NonSerialized]
        public Tile defaultTile;
        public void OnEnable() {
            if (ruleSprites == null || ruleSprites.Length == 0) {
                return;
            }
            
            tiles = new Tile[ruleSprites.Length];
            for (int i = 0; i < ruleSprites.Length; i++) {
                var sprite = ruleSprites[i];
                var tile = CreateInstance<Tile>();
                tile.sprite = sprite;
                tiles[i] = tile;

                if (defaultSprite == sprite) {
                    defaultTile = tile;
                }
            }
        }
    }
}