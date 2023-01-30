using System.Collections.Generic;
using SetupNS;
using GameNS;
using UnityEngine;

namespace WorldNS {
    public static class EntityHelper {
        public static List<Entity> GetOverlappingEntities(SetupEntity setupEntity, Vector2Int field) {
            var result = new List<Entity>();

            var entities = ChunkManager.Instance.EnumerateEntities(field);

            foreach (var entity in entities) {
                if (
                    !setupEntity.blockField &&
                    setupEntity == entity.setup
                ) {
                    continue; //No need to check here, since they may be placed inside each other
                }

                if (
                    setupEntity.blockField &&
                    entity.setup.blockField
                ) {
                    continue; //Placing a blocking entity onto a non-blocking entity is allowed => needs to be cleared somewhere else
                }

                if (entity.OverlapsWith(setupEntity, field)) {
                    result.Add(entity);
                }
            }

            return result;
        }
    }
}