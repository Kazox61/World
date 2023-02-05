using System.Collections.Generic;
using SaveSystemNS;
using ServiceNS;
using SetupNS;
using UnityEngine;
using Terrain = SetupNS.Terrain;

namespace WorldNS {
    public class FieldController : PoolableObject {
        public Vector2Int field;
        
        public Terrain ground;
        public Terrain grass;
        public Terrain decoration;
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
            ground = Terrain.CreateTerrain(SetupCore.GetTerrainSetup("Dirt"), this);
            grass = null;
            decoration = null;
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