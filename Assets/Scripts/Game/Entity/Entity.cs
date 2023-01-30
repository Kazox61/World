using Extensions;
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

        public Vector2Int Field => (Vector2Int)Vector3Int.FloorToInt(transform.position);

        public static Entity Create(SetupEntity setup, Vector3 position) {
            var entity = GameObjectPool.Instance.Get<Entity>(setup.prefab);
            entity.Initialize(position);
            entity.setup = setup;
            return entity;
        }
        
        private void Initialize(Vector3 position) {
            transform.position = position;
        }
        
        public void Remove() {
            GameObjectPool.Instance.Release(gameObject, setup.prefab);
        }

        public bool OverlapsWith(SetupEntity setupEntity, Vector2Int field) {
            var otherRect = setupEntity.GetRect(field);

            return OverlapsWith(otherRect);
        }

        public bool OverlapsWith(Rect otherRect) {
            var ownRect = setup.GetRect(Field);

            return ownRect.OverlapsWith(otherRect);
        }
    }
}