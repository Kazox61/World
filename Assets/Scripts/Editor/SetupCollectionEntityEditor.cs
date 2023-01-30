using SetupNS;
using UnityEditor;
using UnityEngine;

namespace EditorNS {
    [CustomEditor(typeof(SetupCollectionEntity))]
    public class SetupCollectionEntityEditor : Editor {
        
        public SetupCollectionEntity SetupCollectionTerrain => target as SetupCollectionEntity;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Refresh")) {
                SetupCollectionTerrain.setupCollection = Resources.FindObjectsOfTypeAll<SetupEntity>();
            }
        }
    }
}