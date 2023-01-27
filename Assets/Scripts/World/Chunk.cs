using System;
using System.Collections;
using System.Collections.Generic;
using GameNS.Config;
using GameNS.Entity;
using UnityEngine;

namespace WorldNS {
    public class Chunk {
        public Vector2Int chunkPosition;
        
        public Field[] fields;
        public EntityData[] entityData;
        private List<Entity> entities;

        public bool isConstructed;

        public Chunk(Vector2Int chunkPos) {
            chunkPosition = chunkPos;
            entityData = Array.Empty<EntityData>();
            entities = new List<Entity>();
            
            var arraySize = ChunkHelper.CHUNK_SIZE * ChunkHelper.CHUNK_SIZE;
            fields = new Field[arraySize];
            for (int i = 0; i < arraySize; i++) {
                fields[i] = new Field();
            }
        }
        
        
        public static Chunk Create(ChunkData chunkData) {
            var chunk = new Chunk(new Vector2Int(chunkData.sector.x, chunkData.sector.y)) {
                fields = chunkData.fields,
                entityData = chunkData.entities
            };
            return chunk;
        }
        
        public IEnumerator Construct() {
            for (var i = 0; i < fields.Length; i++) {
                var field = fields[i];

                var position = ChunkHelper.FieldArrayIndexToPosition(i,
                    ChunkHelper.ChunkPositionToInitialFieldPosition(chunkPosition));
                TerrainController.Instance.TryPlaceField(position, field);
                yield return null;
            }

            foreach (var data in entityData) {
                FieldController.Instance.TryCreateEntity(
                    EntityConfigCore.GetConfig(data.name),
                    new Vector2(data.position.x, data.position.y),
                    out var _);
                yield return null;
            }
        }

        public IEnumerator Destruct() {
            entityData = new EntityData[entities.Count];
            var clone = new List<Entity>(entities);
            for (int i = 0; i < clone.Count; i++) {
                var entity = clone[i];
                var data = new EntityData(entity);
                FieldController.Instance.RemoveEntity(entity);
                entityData[i] = data;
                yield return null;
            }

            for (int i = 0; i < fields.Length; i++) {
                var field = fields[i];
                var position = ChunkHelper.FieldArrayIndexToPosition(i,
                    ChunkHelper.ChunkPositionToInitialFieldPosition(chunkPosition));
                TerrainController.Instance.TryRemoveField(position, field);
                yield return null;
            }
        }

        public void AddEntity(Entity entity) {
            entities.Add(entity);
        }

        public void RemoveEntity(Entity entity) {
            entities.Remove(entity);
        }

        public void AddField(Vector2Int fieldPosition, Field field) {
            var pos = fieldPosition - ChunkHelper.ChunkPositionToInitialFieldPosition(chunkPosition);
            var index = pos.x + pos.y * ChunkHelper.CHUNK_SIZE;
            fields[index] = field;
        }

        public void RemoveField(Vector2Int fieldPosition) {
            var pos = fieldPosition - ChunkHelper.ChunkPositionToInitialFieldPosition(chunkPosition);
            var index = pos.x + pos.y * ChunkHelper.CHUNK_SIZE;
            fields[index] = new Field();
        }
    }
}