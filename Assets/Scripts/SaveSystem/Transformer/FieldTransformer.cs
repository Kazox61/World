using SetupNS;
using UnityEngine;
using WorldNS;
using Terrain = SetupNS.Terrain;

namespace SaveSystemNS {
	public class FieldTransformer {
		private readonly FieldController fieldController;

		public FieldTransformer(FieldController fieldController) {
			this.fieldController = fieldController;
		}

		public DataField ToData() {
			var ground = fieldController.ground == null ? "" : fieldController.ground.terrainSetup.key;
			var grass = fieldController.grass == null ? "" : fieldController.grass.terrainSetup.key;
			var decoration = fieldController.decoration == null ? "" : fieldController.decoration.terrainSetup.key;
			return new DataField() {
				ground = ground,
				grass = grass,
				decoration = decoration,
				entities = EnumerateEntities()
			};
		}

		private DataEntity[] EnumerateEntities() {
			var entities = fieldController.entities;
			var data = new DataEntity[entities.Count];

			for (int i = 0; i < entities.Count; i++) {
				data[i] = entities[i].entityTransformer.ToData();
			}

			return data;
		}

		public void FromData(DataField dataField, Vector2Int field) {
			fieldController.Initialize(field);

			fieldController.ground =
				Terrain.CreateTerrain(SetupCore.GetTerrainSetup(dataField.ground), fieldController);
			fieldController.grass = Terrain.CreateTerrain(SetupCore.GetTerrainSetup(dataField.grass), fieldController);
			fieldController.decoration =
				Terrain.CreateTerrain(SetupCore.GetTerrainSetup(dataField.decoration), fieldController);
			foreach (var dataEntity in dataField.entities) {
				var setup = SetupCore.GetEntitySetup(dataEntity.name);
				var entity = Entity.CreateEntity(setup,
					GridHelper.PositionToField(new Vector2(dataEntity.position.x, dataEntity.position.y)));
				fieldController.AddEntity(entity);
			}
		}
	}
}