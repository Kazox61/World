using System;
using UnityEngine;

namespace ServiceNS {
	public class ServiceHelper : MonoBehaviour {
		public static ServiceHelper Instance { get; private set; }

		public void Awake() {
			Instance = this;
		}
	}
}