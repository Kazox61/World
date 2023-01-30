using System.Collections.Generic;
using GameNS;
using UnityEngine;

namespace DefaultNamespace {
    public class Overlap : MonoBehaviour {
        public List<Entity> GetEntitiesInArea(Vector2 position, Vector2 size) {
            var entities = new List<Entity>();
            var colliders = Physics2D.OverlapBoxAll(position, size, 0);
            foreach (var col in colliders) {
                var entity = col.GetComponent<Entity>();
                if (entity == null) {
                    continue;
                }
                entities.Add(entity);
            }

            return entities;
        }

        public bool IsAreaBlocked(Vector2 position, Vector2 size) {
            var entities = GetEntitiesInArea(position, size);
            foreach (var entity in entities) {
                if (entity.setup.blockField) {
                    return true;
                }
            }
            return false;
        }
        
    }
}