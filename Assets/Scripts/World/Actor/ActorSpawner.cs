using ServiceNS;
using UnityEngine;
using SetupNS;

namespace WorldNS {
    public class ActorSpawner : MonoBehaviour {
        public string setupKey = "Player";
        private void Start() {
            var setup = SetupCore.GetSetup<ActorSetup>(setupKey);
            Delay.Start(() => {
                Actor.Create(setup, Vector3.zero);
            }, 3);
        }
    }
}