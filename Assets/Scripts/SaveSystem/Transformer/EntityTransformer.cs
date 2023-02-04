using WorldNS;

namespace SaveSystemNS {
    public class EntityTransformer {
        private readonly Entity entity;

        public EntityTransformer(Entity entity) {
            this.entity = entity;
        }

        public DataEntity ToData() {
            var position = entity.transform.position;
            var data = new DataEntity() {
                name = entity.setup.key,
                position = new Position(position.x, position.y)
            };
            entity.Remove();
            return data;
        }
    }
}