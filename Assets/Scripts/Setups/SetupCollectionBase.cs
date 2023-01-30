using System;
using System.Linq;
using UnityEngine;

namespace SetupNS {
    public class SetupCollectionBase<T> : ScriptableObject where T: SetupBase {
        public T[] setupCollection;
        
        public T GetSetup(string key) {
            var setup = setupCollection.FirstOrDefault(setup => setup.key == key);
            if (setup == null) {
                throw new Exception($"There is no Setup with name \"{key}\"!");
            }

            return setup;
        }
    }
}