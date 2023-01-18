using ServiceNS;
using UnityEngine;
using GameNS.Config;

namespace GameNS.Actor {
    public class ActorSpawner : MonoBehaviour {
        public string configName = "Player";
        private void Start() {
            var config = ActorConfigCore.GetConfig(configName);
            Delay.Start(() => {
                Actor.CreateActor(config, Vector3.zero);
            }, 3);
        }
    }
}