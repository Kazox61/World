using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using SetupNS;
using GameNS;
using GameNS.WorldEditor;
using Newtonsoft.Json;
using SaveSystemNS;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldNS {
    public static class ChunkLoader {
        public static void Save(List<Chunk> chunks) {
            var data = new List<DataChunk>();
            foreach (var chunk in chunks) {
                var chunkData = ChunkTransformer.ToData(chunk);
                data.Add(chunkData);
            }
            
            var dataString = JsonConvert.SerializeObject(data);
            var fileStream = new FileStream("Assets/Resources/save/chunks.json", FileMode.Create);
            using var writer = new StreamWriter(fileStream);
            writer.Write(dataString);
        }

        public static Chunk[] Load() {
            var chunks = Array.Empty<Chunk>();
            var file = Resources.Load<TextAsset>("save/chunks");
            if (file == null) {
                return chunks;
            }
            var fileData = file.text;
            var data = JsonConvert.DeserializeObject<DataChunk[]>(fileData);
            if (data == null) {
                return chunks;
            }
            chunks = new Chunk[data.Length];
            for (int i = 0; i < data.Length; i++) {
                var chunkData = data[i];
                chunks[i] = ChunkTransformer.FromData(chunkData);
            }
            return chunks;
        }
    }
}