using System.Collections.Generic;
using SaveSystemNS;
using ServiceNS;
using SetupNS;
using UnityEngine;

namespace WorldNS {
    public class FieldController : PoolableObject {
        public Vector2Int field;
        //TODO: Rework this, move to own class
        public SetupTerrain terrainGround;
        public SetupTerrain terrainGrass;
        public SetupTerrain terrainDecoration;
        public readonly List<Entity> entities = new();

        public FieldTransformer fieldTransformer;
        private bool constructed;
        
        public static FieldController Create(Vector2Int field) {
            var fieldController = ObjectPool<FieldController>.Get();
            fieldController.Construct();
            fieldController.Initialize(field);
            return fieldController;
        }

        public void Construct() {
            if (constructed) {
                return;
            }
            Constructor();
            constructed = true;
        }

        private void Constructor() {
            fieldTransformer = new FieldTransformer(this);
        }
        
        public void Initialize(Vector2Int field) {
            this.field = field;
            terrainGround = SetupCore.GetSetup<SetupTerrain>("Dirt");
            terrainGrass = null;
            terrainDecoration = null;
            entities.Clear();
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