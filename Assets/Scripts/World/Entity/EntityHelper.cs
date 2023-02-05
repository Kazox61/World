using System.Collections.Generic;
using SetupNS;
using UnityEngine;

namespace WorldNS {
    public static class EntityHelper {
        public static List<Entity> GetOverlappingEntities(EntitySetup entitySetup, Vector2Int field) {
            var result = new List<Entity>();

            var entities = ChunkManager.Instance.EnumerateEntities(field);

            foreach (var entity in entities) {
                if (
                    !entitySetup.blockField &&
                    entitySetup == entity.entitySetup
                ) {
                    continue; //No need to check here, since they may be placed inside each other
                }

                if (
                    entitySetup.blockField &&
                    !entity.entitySetup.blockField
                ) {
                    continue; //Placing a blocking entity onto a non-blocking entity is allowed => needs to be cleared somewhere else
                }
                if (entity.OverlapsWith(entitySetup, field)) {
                    result.Add(entity);
                }
            }

            return result;
        }
    }
}