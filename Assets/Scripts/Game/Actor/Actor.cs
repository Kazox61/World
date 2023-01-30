using ServiceNS;
using UnityEngine;
using SetupNS;

namespace GameNS {
    public class Actor: MonoBehaviour {
        public static SetupCollectionBase<SetupActor> SetupCollection =>
            SetupCollectionLoader.SetupCollectionActor;

        public SetupActor setup;
        
        public static Actor Create(SetupActor setupActor, Vector3 position) {
            var actor = GameObjectPool.Instance.Get<Actor>(setupActor.prefab);
            actor.setup = setupActor;
            actor.Initialize(position);
            return actor;
        }

        private void Initialize(Vector3 position) {
            transform.position = position;
        }
    }
}