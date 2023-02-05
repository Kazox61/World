using System.Collections.Generic;
using SetupNS;
using UnityEngine;

namespace WorldNS {
    public abstract class EntityConnected : Entity {
        public SpriteRenderer entityConnectedSpriteRenderer;
        public Sprite[] sprites;
        protected abstract IEnumerable<Rule> GetRules();

        public override void OnStartUp() {
            UpdateNeighbors();
        }

        private void UpdateNeighbors() {
            for (int y = -1; y <= 1; y++) {
                for (int x = -1; x <= 1; x++) {
                    var entity = GetEntity(Field + new Vector2Int(x,y));
                    if (entity == null) continue;
                    UpdateSprite(entity);
                }
            }
        }
        
        private void UpdateSprite(Entity entity) {
            foreach (var rule in GetRules()) {
                if (RuleMatches(rule, entity)) {
                    var entityConnected = (EntityConnected)entity;
                    entityConnected.entityConnectedSpriteRenderer.sprite = sprites[rule.output];
                    return;
                }
            }
        }
        

        private bool RuleMatches(Rule rule, Entity entity) {
            var centerField = entity.Field;
            var index = 0;
            for (int y = -1; y <= 1; y++) {
                for (int x = -1; x <= 1; x++) {
                    if (x != 0 || y != 0) {
                        var offset = new Vector2Int(x, y);
                        var field = centerField + offset;
                        var neighbor = GetEntity(field);
                        var isSameType = neighbor != null && neighbor.entitySetup.key == entity.entitySetup.key;
                        if ((rule.input[index] == 1 && !isSameType) || (rule.input[index] == 2 && isSameType)) {
                            return false;
                        }

                        index++;
                    }
                }
				
            }
            return true;
        }
    }
}