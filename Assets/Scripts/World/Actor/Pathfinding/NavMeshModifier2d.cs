using System.Collections.Generic;
using UnityEngine;

namespace WorldNS {
    public class NavMeshModifier2d : MonoBehaviour {
        private static readonly List<NavMeshModifier2d> activeModifiers = new List<NavMeshModifier2d>();
        public static List<NavMeshModifier2d> ActiveModifiers => activeModifiers;

        public Collider2D navMeshCollider;
        
        private void OnEnable() {
            if (!activeModifiers.Contains(this)) {
                activeModifiers.Add(this);
            }
        }

        private void OnDisable() {
            activeModifiers.Remove(this);
        }
    }
}