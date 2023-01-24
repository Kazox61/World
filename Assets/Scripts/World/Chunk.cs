using System;
using UnityEngine;

namespace WorldNS {
    public class Chunk {
        public Vector2Int chunkPos;
        [System.NonSerialized]
        public BoundsInt bounds;
        public Field[] fields;
        public EntityData[] entities;

        public Chunk(Vector2Int chunkPos) {
            this.chunkPos = chunkPos;
            this.entities = Array.Empty<EntityData>();
            var refPoint = Environment.GetRefPoint(chunkPos);
            var totalSize = Environment.CHUNK_SIZE * Environment.CHUNK_SIZE;
            
            bounds = new BoundsInt(refPoint.x, refPoint.y, 0, Environment.CHUNK_SIZE, Environment.CHUNK_SIZE, 1);
            fields = new Field[totalSize];
            
            for (int i = 0; i < totalSize; i++) {
                fields[i] = new Field();
            }
        }
    }
}