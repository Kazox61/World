using UnityEngine;

namespace WorldNS {
    
    [System.Serializable]
    public class Field {
        public Area area;
        public Vector2Int position;

        public Field(Vector2 position, Area area) {
            this.position = Vector2Int.FloorToInt(position);
            this.area = area;
        }
    }
}