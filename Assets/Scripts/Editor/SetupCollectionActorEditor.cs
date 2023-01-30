using SetupNS;
using UnityEditor;
using UnityEngine;

namespace EditorNS {
    [CustomEditor(typeof(SetupCollectionActor))]
    public class SetupCollectionActorEditor : Editor {
        
        public SetupCollectionActor SetupCollectionTerrain => target as SetupCollectionActor;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Refresh")) {
                SetupCollectionTerrain.setupCollection = Resources.FindObjectsOfTypeAll<SetupActor>();
            }
        }
    }
}