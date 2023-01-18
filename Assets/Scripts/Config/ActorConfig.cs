using UnityEngine;

namespace GameNS.Config {
    [CreateAssetMenu(fileName = "ActorConfigBase", menuName = "ScriptableObjects/ActorConfig", order = 0)]
    public class ActorConfig : ScriptableObject {
        public string configName;
        public GameObject prefab;
    }
}