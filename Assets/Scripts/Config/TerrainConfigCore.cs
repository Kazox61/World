using System.Linq;
using UnityEngine;

namespace GameNS.Config {
    public static class TerrainConfigCore {
        private static TerrainConfig[] configs;
        
        [RuntimeInitializeOnLoadMethod]
        public static void OnLoad() {
            configs = Resources.LoadAll<TerrainConfig>("ScriptableObjects/Terrain");
        }

        public static TerrainConfig GetConfig(string name) {
            return configs.FirstOrDefault(config => config.configName == name);
        }
    }
}