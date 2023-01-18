using System;
using UnityEngine;

namespace ServiceNS {
    public class ServiceHelper : MonoBehaviour {
        public static ServiceHelper instance;

        public void Awake() {
            instance = this;
        }
    }
}