using System.Collections.Generic;
using GameNS;
using UnityEngine;

namespace SaveSystemNS {
    public static class ChunkTransformer {
        
        
        public static DataChunk ToData(Chunk chunk) {
            var data = new DataChunk();
            data.sector = new Sector(chunk.position.x, chunk.position.y);
            data.fields = chunk.fields;
            data.entities = CreateDataEntities(chunk.entities.ToArray());

            return data;
        }

        public static Chunk FromData(DataChunk dataChunk) {
            var chunk = new Chunk(new Vector2Int(dataChunk.sector.x, dataChunk.sector.y));
            chunk.fields = dataChunk.fields;
            //chunk.dataEntities = dataChunk.entities;
            return chunk;
        }

        private static DataEntity[] CreateDataEntities(Entity[] entities) {
            var dataEntities = new List<DataEntity>();
            foreach (var entity in entities) {
                dataEntities.Add(EntityTransformer.ToData(entity));
            }

            return dataEntities.ToArray();
        }
    }
}