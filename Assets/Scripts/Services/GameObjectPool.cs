using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServiceNS {
    public class GameObjectPool {
        private Dictionary<GameObject, Stack<GameObject>> poolRegister = new Dictionary<GameObject, Stack<GameObject>>();

        public static GameObjectPool Instance { get; private set; }


        [RuntimeInitializeOnLoadMethod]
        public static void OnLoad() {
            Instance = new GameObjectPool();
        }


        public T Get<T>(GameObject prefab) where T: MonoBehaviour{
            GameObject gameObject;
            if (!poolRegister.TryGetValue(prefab, out var pool)) {
                pool = new Stack<GameObject>();
                poolRegister.Add(prefab, pool);
            }

            if (pool.Count > 0) {
                gameObject = pool.Pop();
                gameObject.SetActive(true);
                return gameObject.GetComponent<T>();
            }

            gameObject = GameObject.Instantiate(prefab);
            return gameObject.GetComponent<T>();
        }

        public void ReleaseToPool(GameObject gameObject, GameObject prefab) {
            poolRegister.TryGetValue(prefab, out Stack<GameObject> pool);

            if (pool.Contains(gameObject)) {
                throw new Exception($"Object of Type {prefab.name} was added to pool but already exists there");
            }
            
            gameObject.SetActive(false);
            pool.Push(gameObject);
        }
    }
}