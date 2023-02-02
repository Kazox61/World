using System.Collections.Generic;
using UnityEngine;
using WorldNS;

namespace SaveSystemNS {
    //TODO: Do it for all data the same.
    public class ChunkTransformer {
        private Chunk chunk;

        public ChunkTransformer(Chunk chunk) {
            this.chunk = chunk;
        }

        public DataChunk ToData() {
            var data = new DataChunk {
                sector = new Sector(chunk.position.x, chunk.position.y),
                fields = TransformFields()
            };
            return data;
        }

        public static Chunk FromData(DataChunk dataChunk) {
            var position = new Vector2Int(dataChunk.sector.x, dataChunk.sector.y);
            Debug.Log(dataChunk.fields);
            var chunk = new Chunk(
                position,
                ChunkManager.CHUNK_SIZE
            ) {
                fieldControllers = dataChunk.ToFieldControllers()
            };

            return chunk;
        }

        public DataField[] TransformFields() {
            var dataFields = new List<DataField>();
            foreach (var fieldController in chunk.fieldControllers) {
                dataFields.Add(DataField.ToData(fieldController));
            }

            return dataFields.ToArray();
        }
    }
}