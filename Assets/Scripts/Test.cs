using SaveSystemNS;
using UnityEngine;
using WorldNS;

namespace DefaultNamespace {
    public class Test : MonoBehaviour {
        public Camera camera;
        public void Update() {
            var field = GridHelper.PositionToField(camera.transform.position);
            ChunkManager.Instance.UpdateChunks(field);
        }

        public void Save() {
            SaveSystem.Instance.Save(ChunkManager.Instance.GetAllStoredData());
        }
    }
}