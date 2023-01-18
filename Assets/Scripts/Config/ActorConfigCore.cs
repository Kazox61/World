using System;
using System.Linq;
using UnityEngine;

namespace GameNS.Config {
    public static class ActorConfigCore {
        private static ActorConfig[] actorConfigs;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void OnLoad() {
            actorConfigs = Resources.LoadAll<ActorConfig>("ScriptableObjects/Actor");
        }

        public static ActorConfig GetConfig(string name) {
            var config = actorConfigs.FirstOrDefault(actorConfig => actorConfig.configName == name);
            if (config == null) {
                throw new Exception($"There is no ActorConfig with name \"{name}\"!");
            }

            return config;
        }
        
        
    }
}