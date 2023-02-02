using System.Collections;
using System.Collections.Generic;
using SaveSystemNS;
using SetupNS;
using UnityEngine;

namespace WorldNS {
    [System.Serializable]
    public class Chunk {
        public Vector2Int position;
        public int size;
        public Vector2Int originField; // lower left field in this chunk
        public Rect rect;
        public FieldController[] fieldControllers;

        private float keepaliveTime;

        public readonly ChunkTransformer transformer;

        public Chunk(Vector2Int position, int size) {
            var defaultTerrain = SetupCore.GetSetup<SetupTerrain>("Dirt");
            this.position = position;
            this.size = size;
            originField = position * size;
            rect = new Rect(originField.x, originField.y, size, size);
            //@TODO: Don t need to create fieldControllers when Chunk is created from Data
            fieldControllers = new FieldController[size * size];

            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    var index = x + size * y;
                    var field = originField + new Vector2Int(x, y);
                    var fieldController = new FieldController(field);
                    fieldController.terrainGround = defaultTerrain;
                    fieldControllers[index] = fieldController;
                }
            }
            Keepalive();

            transformer = new ChunkTransformer(this);
        }

        public void Keepalive() {
            keepaliveTime = 5;
        }

        public bool UpdateKeepalive() {
            keepaliveTime -= Time.deltaTime;
            return keepaliveTime <= 0;
        }
    }
}