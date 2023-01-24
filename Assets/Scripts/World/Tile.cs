using UnityEngine;

namespace WorldNS {
    
    [System.Serializable]
    public class Tile {
        public Area area;
        public Vector2Int position;

        public Tile(Vector2 position, Area area) {
            this.position = Vector2Int.FloorToInt(position);
            this.area = area;
        }
    }
}