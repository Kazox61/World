using ServiceNS;
using UnityEngine;
using SetupNS;

namespace GameNS {
    public class ActorSpawner : MonoBehaviour {
        public string setupKey = "Player";
        private void Start() {
            var setup = Actor.SetupCollection.GetSetup(setupKey);
            Delay.Start(() => {
                Actor.Create(setup, Vector3.zero);
            }, 3);
        }
    }
}