using System.Collections.Generic;
using UnityEngine;

namespace GameNS.Entity {
    [CreateAssetMenu(fileName = "entityModification", menuName = "ScriptableObjects/EntityModification", order = 0)]
    public class EntityModification : ScriptableObject {
        public Sprite defaultSprite;
        public List<Rule> rules;
    }
}