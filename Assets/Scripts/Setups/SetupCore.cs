using System.Collections.Generic;
using UnityEngine;

namespace SetupNS {
    public static class SetupCore {
        private static readonly Dictionary<string, TerrainSetup> TerrainSetups = new();
        private static readonly Dictionary<string, EntitySetup> EntitySetups = new();
        private static readonly Dictionary<string, ActorSetup> ActorSetups = new();

        [RuntimeInitializeOnLoadMethod]
        public static void OnLoad() {
            var setups = Resources.Load<SetupCollection>("SetupCollection");
            foreach (var setup in setups.terrainSetups) {
                TerrainSetups.Add(setup.key, setup);
            }
            foreach (var setup in setups.entitySetups) {
                EntitySetups.Add(setup.key, setup);
            }
            foreach (var setup in setups.actorSetups) {
                ActorSetups.Add(setup.key, setup);
            }
        }

        public static TerrainSetup GetTerrainSetup(string key) {
            var success = TerrainSetups.TryGetValue(key, out var setup);
            return !success ? null : setup;
        }
        
        public static EntitySetup GetEntitySetup(string key) {
            var success = EntitySetups.TryGetValue(key, out var setup);
            return !success ? null : setup;
        }
        
        public static ActorSetup GetActorSetup(string key) {
            var success = ActorSetups.TryGetValue(key, out var setup);
            return !success ? null : setup;
        }
    }
}