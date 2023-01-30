using SetupNS;
using UnityEditor;
using UnityEngine;

namespace EditorNS {
    [CustomEditor(typeof(SetupCollectionTerrain))]
    public class SetupCollectionTerrainEditor : Editor {
        
        public SetupCollectionTerrain SetupCollectionTerrain => target as SetupCollectionTerrain;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Refresh")) {
                SetupCollectionTerrain.setupCollection = Resources.FindObjectsOfTypeAll<SetupTerrain>();
            }
        }
    }
}