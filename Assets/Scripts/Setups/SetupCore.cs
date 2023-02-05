using System.Collections.Generic;
using UnityEngine;

namespace SetupNS {
    public static class SetupCore {
        private static readonly Dictionary<string, SetupBase> Setups = new();

        [RuntimeInitializeOnLoadMethod]
        public static void OnLoad() {
            var setups = Resources.Load<SetupCollection>("SetupCollection");
            foreach (var setup in setups.setups) {
                Setups.Add(setup.key, setup);
            }
        }

        public static T GetSetup<T>(string key) where T: SetupBase {
            var success = Setups.TryGetValue(key, out var setup);
            if (!success) {
                return null;
            }
            return (T)setup;
        }
    }
}