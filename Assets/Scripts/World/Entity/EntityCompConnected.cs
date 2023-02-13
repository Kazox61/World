using System.Collections.Generic;
using SetupNS;
using UnityEngine;

namespace WorldNS {
	public abstract class EntityCompConnectedBase : EntityCompBase {
		public SpriteRenderer entityConnectedSpriteRenderer;
		public Sprite[] sprites;
		protected abstract IEnumerable<Rule> GetRules();

		public void UpdateNeighbors() {
			for (int y = -1; y <= 1; y++) {
				for (int x = -1; x <= 1; x++) {
					var neighborEntity = Entity.GetEntity(entity.Field + new Vector2Int(x, y));
					if (neighborEntity == null || neighborEntity.composites.entityCompConnected == null) continue;
					UpdateSprite(neighborEntity);
				}
			}
		}

		private void UpdateSprite(Entity entityConnected) {
			foreach (var rule in GetRules()) {
				if (RuleMatches(rule, entityConnected)) {
					var spriteRenderer = entityConnected.composites.entityCompConnected.entityConnectedSpriteRenderer;
					spriteRenderer.sprite = sprites[rule.output];
					return;
				}
			}
		}


		private bool RuleMatches(Rule rule, Entity entityConnected) {
			var centerField = entityConnected.Field;
			var index = 0;
			for (int y = -1; y <= 1; y++) {
				for (int x = -1; x <= 1; x++) {
					if (x != 0 || y != 0) {
						var offset = new Vector2Int(x, y);
						var field = centerField + offset;
						var neighborEntity = Entity.GetEntity(field);
						var isSameType = neighborEntity != null &&
						                 neighborEntity.entitySetup.key == entityConnected.entitySetup.key;
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