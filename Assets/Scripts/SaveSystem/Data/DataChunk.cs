using System.Collections.Generic;
using GameNS;
using UnityEngine;
using WorldNS;

namespace SaveSystemNS {
    public struct DataChunk {
        public Sector sector;
        public DataField[] fields;

        public FieldController[] ToFieldControllers() {
            var index = 0;
            var fieldControllers = new List<FieldController>();
            foreach (var dataField in fields) {
                var x = index % ChunkManager.CHUNK_SIZE + sector.x * ChunkManager.CHUNK_SIZE;
                var y = index / ChunkManager.CHUNK_SIZE + sector.y * ChunkManager.CHUNK_SIZE;
                var fieldController = dataField.ToFieldController(new Vector2Int(x,y));
                fieldControllers.Add(fieldController);
                index++;
            }

            return fieldControllers.ToArray();
        }
    }
}