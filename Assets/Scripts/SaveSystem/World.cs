using System;

namespace SaveSystemNS {
    public class World {
        public DataChunk[] chunks;

        //@TODO Load from Json
        public void Create() {
            chunks = Array.Empty<DataChunk>();
        }
    }
}