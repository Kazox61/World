using System;
using System.Collections;
using System.Collections.Generic;
using SetupNS;
using GameNS;
using SaveSystemNS;
using UnityEngine;

namespace WorldNS {
    [System.Serializable]
    public class Chunk {
        public Vector2Int position;
        
        public Field[] fields;
        public List<Entity> entities;
        public FieldController[,] fieldControllers;

        public IEnumerator coroutineDestruct;
        public IEnumerator coroutineConstruct;

        public Chunk(Vector2Int pos) {
            position = pos;
            entities = new List<Entity>();

            var initialField = ChunkHelper.ChunkPositionToInitialFieldPosition(pos);

            fieldControllers = new FieldController[ChunkHelper.CHUNK_SIZE, ChunkHelper.CHUNK_SIZE];
            for (int y = 0; y < fieldControllers.GetLength(1); y++) {
                for (int x = 0; x < fieldControllers.GetLength(0); x++) {
                    var field = initialField + new Vector2Int(x, y);
                    var dirt = SetupCollectionLoader.SetupCollectionTerrain.GetSetup("Dirt");
                    SetupTerrain.CreateTerrain(dirt, field);
                    
                    fieldControllers[x, y] = new FieldController(field);
                }
            }
        }

        public IEnumerator Construct() { yield return null; }

        public IEnumerator Destruct() { yield return null; }

        public void AddEntity(Entity entity) {
            entities.Add(entity);
        }

        public void RemoveEntity(Entity entity) {
            entities.Remove(entity);
        }
    }
}