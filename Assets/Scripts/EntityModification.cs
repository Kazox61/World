using System.Collections.Generic;
using UnityEngine;

namespace SetupNS {
    [CreateAssetMenu(fileName = "entityModification", menuName = "ScriptableObjects/EntityModification", order = 0)]
    public class EntityModification : ScriptableObject {
        public Sprite defaultSprite;
        public List<GameNS.ModificationRule> rules;
    }
}