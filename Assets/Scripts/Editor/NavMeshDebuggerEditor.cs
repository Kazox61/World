using GameNS;
using UnityEditor;
using UnityEngine;

namespace EditorNS {
    [CustomEditor(typeof(NavMeshDebugger))]
    public class NavMeshDebuggerEditor : Editor {
        public NavMeshDebugger NavMeshDebugger => target as NavMeshDebugger;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Bake")) {
                NavMeshPath2D.Instance.BuildNavMesh(NavMeshDebugger.centerPosition, NavMeshDebugger.size);
            }
        }
    }
}