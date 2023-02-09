using UnityEngine;

namespace SetupNS {
    public abstract class SetupBase : ScriptableObject {
        public string key;
        public Sprite defaultSprite;
        public GameObject prefab;
    }
}