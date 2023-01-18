using ServiceNS;
using UnityEngine;
using GameNS.Config;

namespace GameNS.Actor {
    public class Actor: MonoBehaviour {

        public ActorConfig config;
        
        public static Actor CreateActor(ActorConfig actorConfig, Vector3 position) {
            var actor = GameObjectPool.Instance.Get<Actor>(actorConfig.prefab);
            actor.config = actorConfig;
            actor.Initialize(position);
            return actor;
        }

        private void Initialize(Vector3 position) {
            transform.position = position;
        }
    }
}