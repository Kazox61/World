using ServiceNS;
using UnityEngine;
using WorldNS;

namespace SaveSystemNS {
    public class ChunkTransformer {
        private readonly Chunk chunk;

        public ChunkTransformer(Chunk chunk) {
            this.chunk = chunk;
        }

        public DataChunk ToData() {
            return new DataChunk {
                sector = new Sector(chunk.position.x, chunk.position.y),
                fields = EnumerateFields()
            };
        }

        private DataField[] EnumerateFields() {
            var fields = chunk.fieldControllers;
            var data = new DataField[fields.Length];
            for (int i = 0; i < fields.Length; i++) {
                data[i] = fields[i].fieldTransformer.ToData();
            }

            return data;
        }
        
        public void FromData(DataChunk dataChunk) {
            var position = new Vector2Int(dataChunk.sector.x, dataChunk.sector.y);
            chunk.Construct();
            chunk.Initialize(position);
            
            for (int i = 0; i < dataChunk.fields.Length; i++) {
                var dataField = dataChunk.fields[i];
                var fieldController = ObjectPool<FieldController>.Get();
                fieldController.Construct();
                fieldController.fieldTransformer.FromData(dataField, ChunkHelper.FieldIndexToField(chunk.position, i));
                chunk.fieldControllers[i] = fieldController;
            }
        }
    }
}