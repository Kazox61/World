using System;
using UnityEngine;
using WorldNS;

namespace DefaultNamespace {
    public class Test : MonoBehaviour {
        public Camera camera;
        public void Update() {
            var field = GridHelper.PositionToField(camera.transform.position);
            ChunkManager.Instance.UpdateChunks(field);
        }
    }
}