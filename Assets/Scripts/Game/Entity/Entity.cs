using UnityEngine;
using ServiceNS;
using WorldNS;
using SetupNS;

namespace GameNS {
    public class Entity: MonoBehaviour {
        public static SetupCollectionBase<SetupEntity> SetupCollection =>
            SetupCollectionLoader.SetupCollectionEntity;

        public SetupEntity setup;
        public SpriteRenderer spriteRenderer;

        public Vector2Int Field => GridHelper.PositionToField(transform.position);

        private void Initialize(Vector2Int field) {
            var center = GridHelper.FieldToPosition(field);
            transform.position = center;
        }
        
        public static Entity Create(SetupEntity setup, Vector2Int field) {
            var entity = GameObjectPool.Instance.Get<Entity>(setup.prefab);
            entity.setup = setup;
            entity.Initialize(field);
            return entity;
        }

        public static Entity CreateEntity(SetupEntity setupEntity, Vector2Int field) {
            if (!CanCreateEntity(setupEntity, field)) {
                return null;
            }

            var entity = Create(setupEntity, field);

            var chunkPosition = ChunkHelper.FieldToChunkPosition(field);
            var success = ChunkManager.Instance.TryGetChunk(chunkPosition, out var chunk);
            
            chunk.AddEntity(entity);

            return entity;
        }

        

        public static bool CanCreateEntity(SetupEntity setupEntity, Vector2Int field) {
            var detectionSet = new DetectionSet() { field = field, setupEntity = setupEntity };
            var areaDetection = AreaDetectionBuilder.Instance.GetAreaDetection(setupEntity.ignoreDetectionLayers);
            return areaDetection.IsClean(detectionSet);
        }

        public static Entity GetEntity(Vector2Int field) {
            var entities = ChunkManager.Instance.EnumerateEntities(field);
            var mouseRect = new Rect(field.x, field.y, 1, 1);
            
            foreach (var entity in entities) {
                entity.OverlapsWith(mouseRect);
                return entity;
            }

            return null;
        }

        public bool OverlapsWith(SetupEntity setupEntity, Vector2Int field) {
            var otherRect = setupEntity.GetRect(field);

            return OverlapsWith(otherRect);
        }

        public bool OverlapsWith(Rect otherRect) {
            var ownRect = setup.GetRect(Field);
            Debug.DrawLine(new Vector3(ownRect.xMin, ownRect.yMin), new Vector3(ownRect.xMax, ownRect.yMin), Color.green, 100);
            Debug.DrawLine(new Vector3(ownRect.xMin, ownRect.yMax), new Vector3(ownRect.xMax, ownRect.yMax), Color.green, 100);
            Debug.DrawLine(new Vector3(ownRect.xMin, ownRect.yMin), new Vector3(ownRect.xMin, ownRect.yMax), Color.green, 100);
            Debug.DrawLine(new Vector3(ownRect.xMax, ownRect.yMin), new Vector3(ownRect.xMax, ownRect.yMax), Color.green, 100);
            
            Debug.DrawLine(new Vector3(otherRect.xMin, otherRect.yMin), new Vector3(otherRect.xMax, otherRect.yMin), Color.magenta, 100);
            Debug.DrawLine(new Vector3(otherRect.xMin, otherRect.yMax), new Vector3(otherRect.xMax, otherRect.yMax), Color.magenta, 100);
            Debug.DrawLine(new Vector3(otherRect.xMin, otherRect.yMin), new Vector3(otherRect.xMin, otherRect.yMax), Color.magenta, 100);
            Debug.DrawLine(new Vector3(otherRect.xMax, otherRect.yMin), new Vector3(otherRect.xMax, otherRect.yMax), Color.magenta, 100);
            return ownRect.Overlaps(otherRect);
        }
        
        public void Remove() {
            GameObjectPool.Instance.Release(gameObject, setup.prefab);
        }
    }
}