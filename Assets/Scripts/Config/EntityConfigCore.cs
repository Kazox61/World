using System;
using System.Linq;
using UnityEngine;

namespace GameNS.Config {
    public static class EntityConfigCore {
        private static EntityConfig[] entityConfigs;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void OnLoad() {
            entityConfigs = Resources.LoadAll<EntityConfig>("ScriptableObjects/Entity/Config");
        }

        public static EntityConfig GetConfig(string name) {
            var config = entityConfigs.FirstOrDefault(entityConfig => entityConfig.configName == name);
            if (config == null) {
                //throw new Exception($"There is no EntityConfig with name \"{name}\"!");
                return null;
            }

            return config;
        }
        
        
        
        
    }
}