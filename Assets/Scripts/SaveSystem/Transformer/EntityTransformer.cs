using SetupNS;
using GameNS;
using UnityEngine;
using DataEntity = SaveSystemNS.DataEntity;
using Position = SaveSystemNS.Position;

namespace SaveSystemNS {
    public static class EntityTransformer {
        public static DataEntity ToData(Entity entity) {
            var data = new DataEntity();
            data.name = entity.setup.key;
            data.position = new Position(entity.transform.position.x, entity.transform.position.y);
            return data;
        }

        public static void FromData(DataEntity dataEntity) {
            var config = Entity.SetupCollection.GetSetup(dataEntity.name);
            var position = new Vector2(dataEntity.position.x, dataEntity.position.y);
            //EntityHelper.Instance.TryCreateEntity(config, position, out _);
        }
    }
}