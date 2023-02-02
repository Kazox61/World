using SetupNS;
using UnityEngine;
using WorldNS;

namespace SaveSystemNS {
    public struct DataEntity {
        public string name;
        public Position position;

        public Entity ToEntity() {
            var setup = SetupCore.GetSetup<SetupEntity>(name);
            return Entity.CreateEntity(setup, GridHelper.PositionToField(new Vector2(position.x, position.y)));
        }

        public static DataEntity ToData(Entity entity) {
            var position = entity.transform.position;
            return new DataEntity() {
                name = entity.name,
                position = new Position(position.x, position.y)
            };
        }
    }
}