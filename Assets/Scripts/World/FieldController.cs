using System.Collections.Generic;
using SetupNS;
using UnityEngine;

namespace WorldNS {
    public class FieldController {
        public Vector2Int field;
        //TODO: Rework this, move to own class
        public SetupTerrain terrainGround;
        public SetupTerrain terrainGrass;
        public SetupTerrain terrainDecoration;
        public readonly List<Entity> entities = new();

        public FieldController(Vector2Int field) {
            this.field = field;
        }

        public void AddEntity(Entity entity) {
            entities.Add(entity);
        }
        
        public void RemoveEntities() {
            foreach (var entity in entities) {
                entity.Remove();
            }
            entities.Clear();
        }

        public void RemoveTerrain() {
            
        }
    }
}