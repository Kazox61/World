using ServiceNS;
using UnityEngine;
using SetupNS;

namespace WorldNS {
    public class Actor: MonoBehaviour {
        public ActorSetup actorSetup;
        
        public static Actor Create(ActorSetup actorSetup, Vector3 position) {
            var actor = GameObjectPool.Instance.Get<Actor>(actorSetup.prefab);
            actor.actorSetup = actorSetup;
            actor.Initialize(position);
            return actor;
        }

        private void Initialize(Vector3 position) {
            transform.position = position;
        }
    }
}