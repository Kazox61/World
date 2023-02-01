using UnityEngine;

namespace SetupNS {
    [CreateAssetMenu(fileName = "SetupTerrainModification", menuName = "SetupTerrainModification", order = 0)]
    public class SetupTerrainModification : ScriptableObject {
        public Sprite[] ruleSprites;
        
        [System.NonSerialized]
        public Tile[] tiles;
        
        public void OnBegin() {
            tiles = new Tile[ruleSprites.Length];
            for (int i = 0; i < ruleSprites.Length; i++) {
                var sprite = ruleSprites[i];
                var tile = CreateInstance<Tile>();
                tile.sprite = sprite;
                tiles[i] = tile;
            }
        }
    }
}