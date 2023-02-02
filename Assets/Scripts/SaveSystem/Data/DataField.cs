using System.Collections.Generic;
using SetupNS;
using UnityEngine;
using WorldNS;

namespace SaveSystemNS {
    public struct DataField {
        public string ground;
        public string grass;
        public string decoration;
        public DataEntity[] entities;
        
        public FieldController ToFieldController(Vector2Int field) {
            var fieldController = new FieldController(field);
            foreach (var entity in entities) {
                fieldController.entities.Add(entity.ToEntity());
            }

            fieldController.terrainGround = SetupCore.GetSetup<SetupTerrain>(ground);
            fieldController.terrainGrass = SetupCore.GetSetup<SetupTerrain>(grass);
            fieldController.terrainDecoration = SetupCore.GetSetup<SetupTerrain>(decoration);
            return fieldController;
        }

        public static DataField ToData(FieldController fieldController) {
            var ground = fieldController.terrainGround == null ? "" : fieldController.terrainGround.key;
            var grass = fieldController.terrainGrass == null ? "" : fieldController.terrainGrass.key;
            var decoration = fieldController.terrainDecoration == null ? "" : fieldController.terrainDecoration.key;
            
            var data = new DataField() {
                ground = ground,
                grass = grass,
                decoration = decoration,
                entities = ToEntityData(fieldController.entities)
            };
            return data;
        }

        public static DataEntity[] ToEntityData(List<Entity> entities) {
            var data = new List<DataEntity>();
            foreach (var entity in entities) {
                data.Add(DataEntity.ToData(entity));
            }

            return data.ToArray();
        }
    }
}