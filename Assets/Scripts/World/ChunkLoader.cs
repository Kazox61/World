using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Extensions;
using GameNS.Config;
using GameNS.Entity;
using GameNS.WorldEditor;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldNS {
    public static class ChunkLoader {
        public static void Save(List<Chunk> chunks) {
            var data = new List<ChunkData>();
            foreach (var chunk in chunks) {
                var chunkData = new ChunkData(chunk);
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
            var data = JsonConvert.DeserializeObject<ChunkData[]>(fileData);
            if (data == null) {
                return chunks;
            }
            chunks = new Chunk[data.Length];
            for (int i = 0; i < data.Length; i++) {
                var chunkData = data[i];
                chunks[i] = Chunk.Create(chunkData);
            }
            return chunks;
        }
    }

    public struct ChunkData {
        public Sector sector;
        public Field[] fields;
        public EntityData[] entities;
        
        public ChunkData(Chunk chunk) {
            sector = new Sector(chunk.chunkPosition.x, chunk.chunkPosition.y);
            fields = chunk.fields;
            entities = chunk.entityData;
        }
    }

    public struct EntityData {
        public string name;
        public Position position;

        public EntityData(Entity entity) {
            name = entity.config.configName;
            var pos = entity.transform.position;
            position = new Position(pos.x, pos.y);
        }
    }
    
    public struct Sector {
        public int x;
        public int y;

        public Sector(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
    
    public struct Position {
        public float x;
        public float y;

        public Position(float x, float y) {
            this.x = x;
            this.y = y;
        }
    }
}