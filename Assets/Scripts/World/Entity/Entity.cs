using UnityEngine;
using ServiceNS;
using SaveSystemNS;
using SetupNS;
using Terrain = SetupNS.Terrain;

namespace WorldNS {
	public class Entity : MonoBehaviour {
		public EntityComposites composites;

		[System.NonSerialized] public EntitySetup entitySetup;
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
			if (entitySetup.prefab == null) {
				Terrain.PlaceTerrain(entitySetup.terrain, field);
				return null;
			}

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
			if (entity == null) {
				return null;
			}

			ChunkManager.Instance.AddEntity(field, entity);
			entity.OnStartUp();
			return entity;
		}

		public static bool CanCreateEntity(EntitySetup entitySetup, Vector2Int field) {
			var detectionSet = new DetectionSet() { field = field, entitySetup = entitySetup };
			var areaDetection = AreaDetectionBuilder.Instance.GetAreaDetection(entitySetup.areaDetectionLayers);
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

		public bool OverlapsWith(EntitySetup setup, Vector2Int field) {
			var otherRect = setup.GetRect(field);

			return OverlapsWith(otherRect);
		}

		public bool OverlapsWith(Rect otherRect) {
			var ownRect = entitySetup.GetRect(Field);

			return ownRect.Overlaps(otherRect);
		}

		public void Remove() {
			GameObjectPool.Instance.Release(gameObject, entitySetup.prefab);
		}
	}
}