using System;
using System.Collections;
using UnityEngine;

namespace ServiceNS {
    public static class Delay {
        public static void Start(Action callback, float delay) {
            ServiceHelper.instance.StartCoroutine(Coroutine(callback, delay));
        }

        private static IEnumerator Coroutine(Action callback, float delay) {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }

    }
}