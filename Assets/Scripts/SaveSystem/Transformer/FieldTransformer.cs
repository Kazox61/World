using SetupNS;
using UnityEngine;
using WorldNS;

namespace SaveSystemNS {
    public class FieldTransformer {
        private readonly FieldController fieldController;

        public FieldTransformer(FieldController fieldController) {
            this.fieldController = fieldController;
        }

        public DataField ToData() {
            var ground = fieldController.terrainGround == null ? "" : fieldController.terrainGround.key;
            var grass = fieldController.terrainGrass == null ? "" : fieldController.terrainGrass.key;
            var decoration = fieldController.terrainDecoration == null ? "" : fieldController.terrainDecoration.key;
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
            
            fieldController.terrainGround = SetupCore.GetSetup<SetupTerrain>(dataField.ground);
            fieldController.terrainGrass = SetupCore.GetSetup<SetupTerrain>(dataField.grass);
            fieldController.terrainDecoration = SetupCore.GetSetup<SetupTerrain>(dataField.decoration);
            foreach (var dataEntity in dataField.entities) {
                var setup = SetupCore.GetSetup<SetupEntity>(dataEntity.name);
                var entity = Entity.CreateEntity(setup, GridHelper.PositionToField(new Vector2(dataEntity.position.x, dataEntity.position.y)));
                fieldController.AddEntity(entity);
            }
        }
    }
}