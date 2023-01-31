using SetupNS;
using UnityEditor;
using UnityEngine;

namespace EditorNS {
    [CustomEditor(typeof(SetupCollectionEntity))]
    public class SetupCollectionEntityEditor : Editor {
        
        public SetupCollectionEntity SetupCollectionEntity => target as SetupCollectionEntity;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Refresh")) {
                SetupCollectionEntity.setupCollection = Resources.FindObjectsOfTypeAll<SetupEntity>();
            }
        }
    }
}