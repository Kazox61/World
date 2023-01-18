using UnityEngine;
using ServiceNS;
using WorldNS;
using GameNS.Config;

namespace GameNS.Entity {
    public class Entity: MonoBehaviour {
        public EntityConfig config;
        public SpriteRenderer spriteRenderer;
        
        public static Entity CreateEntity(EntityConfig config, Vector3 position) {
            var entity = GameObjectPool.Instance.Get<Entity>(config.prefab);
            entity.Initialize(position);
            entity.config = config;
            return entity;
        }
        
        private void Initialize(Vector3 position) {
            transform.position = position;
        }

        public void OnNeighborChanged(Vector2Int delta) {
            
        }

        public void UpdateSprite() {
            if (config.modification == null) {
                return;
            }
            foreach (var rule in config.modification.rules) {
                if (RuleMatches(rule)) {
                    spriteRenderer.sprite = rule.sprites[0];
                    spriteRenderer.transform.localPosition = new Vector3(rule.offsetX, rule.offsetY);
                }
            }
        }

        private bool RuleMatches(Rule rule) {
            for (int y = -1; y <= 1; y++) {
                for (int x = -1; x <= 1; x++) {
                    if (x != 0 || y != 0) {
                        var offset = new Vector2Int(x, y);
                        var position = transform.position;
                        var success = FieldController.instance.TryGetEntity(
                            new Vector2(position.x + offset.x, position.y + offset.y),
                            
                            out var entity
                            );
                        var neighborConfigName = "";
                        if (success) {
                            neighborConfigName = entity.config.configName;
                        }
                        var index = GetIndexOfOffset(offset);
                        if ((rule.neighbors[index] == Rule.Neighbor.This && neighborConfigName != config.configName) || (rule.neighbors[index] == Rule.Neighbor.NotThis && neighborConfigName == config.configName)) {
                            return false;
                        }
                    }
                }
				
            }
            return true;
        }
        
        private int GetIndexOfOffset(Vector2Int offset)
        {
            int result = offset.x + 1 + (-offset.y + 1) * 3;
            if (result >= 4)
                result--;
            return result;
        }

        public void ReleaseToPool() {
            GameObjectPool.Instance.ReleaseToPool(gameObject, config.prefab);
        }
    }
}