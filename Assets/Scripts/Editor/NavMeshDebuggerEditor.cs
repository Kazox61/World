using WorldNS;
using UnityEditor;
using UnityEditor.AI;
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

        public void OnEnable() {
            NavMeshVisualizationSettings.showNavigation++;
        }

        public void OnDisable() {
            NavMeshVisualizationSettings.showNavigation--;
        }
    }
}