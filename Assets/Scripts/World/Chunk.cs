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
        public DataEntity[] dataEntities;
        public List<Entity> entities;

        public IEnumerator coroutineDestruct;
        public IEnumerator coroutineConstruct;

        private bool isConstructed;
        

        public Chunk(Vector2Int pos) {
            position = pos;
            dataEntities = Array.Empty<DataEntity>();
            entities = new List<Entity>();
            
            var arraySize = ChunkHelper.CHUNK_SIZE * ChunkHelper.CHUNK_SIZE;
            fields = new Field[arraySize];
            for (int i = 0; i < arraySize; i++) {
                fields[i] = new Field();
            }
        }

        public IEnumerator Construct() {
            while (isConstructed) {
                yield return null;
            }

            var frame = 0;
            for (var i = 0; i < fields.Length; i++) {
                var field = fields[i];

                var fieldPosition = ChunkHelper.FieldArrayIndexToPosition(i,
                    ChunkHelper.ChunkPositionToInitialFieldPosition(position));
                //TerrainController.Instance.SetField(fieldPosition, field);
                
                if (frame >= ChunkHelper.CONSTRUCT_AMOUNT_PER_FRAME || i  >= fields.Length - 1) {
                    frame = 0;
                    yield return null;
                }
                frame++;
            }
            for (int i = 0; i < dataEntities.Length; i++) {
                var data = dataEntities[i];
                //Create Entity
                
                if (frame >= ChunkHelper.CONSTRUCT_AMOUNT_PER_FRAME || i  >= fields.Length - 1) {
                    frame = 0;
                    yield return null;
                }
                frame++;
            }

            coroutineConstruct = null;
            isConstructed = true;
        }

        public IEnumerator Destruct() {
            while (!isConstructed) {
                yield return null;
            }
            
            var frame = 0;
            dataEntities = new DataEntity[entities.Count];
            var clone = new List<Entity>(entities);
            
            for (int i = 0; i < clone.Count; i++) {
                var entity = clone[i];
                var data = EntityTransformer.ToData(entity);
                //RemoveEntity
                dataEntities[i] = data;
                
                if (frame >= ChunkHelper.DESTRUCT_AMOUNT_PER_FRAME || i  >= fields.Length - 1) {
                    frame = 0;
                    yield return null;
                }
                frame++;
            }

            for (int i = 0; i < fields.Length; i++) {
                var fieldPosition = ChunkHelper.FieldArrayIndexToPosition(i,
                    ChunkHelper.ChunkPositionToInitialFieldPosition(position));
                //TerrainController.Instance.ClearField(fieldPosition);
                
                if (frame >= ChunkHelper.DESTRUCT_AMOUNT_PER_FRAME || i  >= fields.Length - 1) {
                    frame = 0;
                    yield return null;
                }
                frame++;
            }

            coroutineDestruct = null;
            isConstructed = false;
        }

        public void AddEntity(Entity entity) {
            entities.Add(entity);
        }

        public void RemoveEntity(Entity entity) {
            entities.Remove(entity);
        }

        public void AddField(Vector2Int fieldPosition, Field field) {
            var pos = fieldPosition - ChunkHelper.ChunkPositionToInitialFieldPosition(position);
            var index = pos.x + pos.y * ChunkHelper.CHUNK_SIZE;
            fields[index] = field;
        }

        public void RemoveField(Vector2Int fieldPosition) {
            var index = ChunkHelper.PositionToFieldArrayIndex(fieldPosition, position);
            fields[index] = new Field();
        }
    }
}