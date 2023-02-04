using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldNS {
    public class NavMeshDebugger : MonoBehaviour {
        public static NavMeshDebugger Instance { get; private set; }
        
        public bool drawPaths = true;
        private readonly List<Vector2[]> paths = new();
        
        [Header("BuildNavMesh")]
        public Vector3 centerPosition = Vector3.zero;
        public Vector2 size = new Vector2(50, 50);

        public void Awake() {
            Instance = this;
        }

        public void AddPathToList(Vector2[] path) {
            paths.Add(path);
        }
        
        public void OnDrawGizmos() {
            Gizmos.color = Color.green;
            if (!drawPaths) {
                return;
            }
            foreach (var path in paths) {
                DrawPath(path);
            }
        }

        private void DrawPath(Vector2[] path) {
            for (int i = 0; i < path.Length; i++) {
                var hasNextPoint = i < path.Length - 1;
                if (!hasNextPoint) {
                    continue;
                }
                Gizmos.DrawLine(path[i], path[i+1]);
            }
        }
    }
}