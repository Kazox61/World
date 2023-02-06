using UnityEngine;
using ServiceNS;
using SaveSystemNS;
using SetupNS;

namespace WorldNS {
    public class Entity: MonoBehaviour {
        public EntityComposites composites;

        [System.NonSerialized]
        public EntitySetup entitySetup;
        public EntityTransformer entityTransformer;
        
        private bool constructed;
        public Vector2Int Field => GridHelper.PositionToField(transform.position);

        private void Initialize(Vector2Int field) {
            transform.position = GridHelper.FieldToPosition(field);
        }

        private void Construct() {
            if (constructed) {
                return;
            }
            Constructor();
            constructed = true;
        }

        private void Constructor() {
            entityTransformer = new EntityTransformer(this);
        }

        public virtual void OnStartUp() {
            if (composites.entityCompConnected != null) {
                composites.entityCompConnected.UpdateNeighbors();
            }
        }
        
        public static Entity CreateEntity(EntitySetup entitySetup, Vector2Int field) {
            var entity = GameObjectPool.Instance.Get<Entity>(entitySetup.prefab);
            entity.entitySetup = entitySetup;
            entity.Initialize(field);
            entity.Construct();
            return entity;
        }

        public static Entity PlaceEntity(EntitySetup entitySetup, Vector2Int field) {
            if (!CanCreateEntity(entitySetup, field)) {
                return null;
            }

            var entity = CreateEntity(entitySetup, field);
            ChunkManager.Instance.AddEntity(field, entity);
            entity.OnStartUp();
            return entity;
        }
        
        public static bool CanCreateEntity(EntitySetup entitySetup, Vector2Int field) {
            var detectionSet = new DetectionSet() { field = field, entitySetup = entitySetup };
            var areaDetection = AreaDetectionBuilder.Instance.GetAreaDetection(entitySetup.ignoreDetectionLayers);
            return areaDetection.IsClean(detectionSet);
        }

        public static Entity GetEntity(Vector2Int field) {
            var entities = ChunkManager.Instance.EnumerateEntities(field);
            var mouseRect = new Rect(field.x, field.y, 1, 1);
            
            foreach (var entity in entities) {
                if (entity.OverlapsWith(mouseRect)) return entity;
            }

            return null;
        }

        public bool OverlapsWith(EntitySetup entitySetup, Vector2Int field) {
            var otherRect = entitySetup.GetRect(field);

            return OverlapsWith(otherRect);
        }

        public bool OverlapsWith(Rect otherRect) {
            var ownRect = entitySetup.GetRect(Field);
            /*
            Debug.DrawLine(new Vector3(ownRect.xMin, ownRect.yMin), new Vector3(ownRect.xMax, ownRect.yMin), Color.green, 100);
            Debug.DrawLine(new Vector3(ownRect.xMin, ownRect.yMax), new Vector3(ownRect.xMax, ownRect.yMax), Color.green, 100);
            Debug.DrawLine(new Vector3(ownRect.xMin, ownRect.yMin), new Vector3(ownRect.xMin, ownRect.yMax), Color.green, 100);
            Debug.DrawLine(new Vector3(ownRect.xMax, ownRect.yMin), new Vector3(ownRect.xMax, ownRect.yMax), Color.green, 100);
            
            Debug.DrawLine(new Vector3(otherRect.xMin, otherRect.yMin), new Vector3(otherRect.xMax, otherRect.yMin), Color.magenta, 100);
            Debug.DrawLine(new Vector3(otherRect.xMin, otherRect.yMax), new Vector3(otherRect.xMax, otherRect.yMax), Color.magenta, 100);
            Debug.DrawLine(new Vector3(otherRect.xMin, otherRect.yMin), new Vector3(otherRect.xMin, otherRect.yMax), Color.magenta, 100);
            Debug.DrawLine(new Vector3(otherRect.xMax, otherRect.yMin), new Vector3(otherRect.xMax, otherRect.yMax), Color.magenta, 100);
            */
            return ownRect.Overlaps(otherRect);
        }
        
        public void Remove() {
            GameObjectPool.Instance.Release(gameObject, entitySetup.prefab);
        }
    }
}