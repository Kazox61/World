using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using GameNS.Entity;
using UnityEngine;

namespace ServiceNS {
    public static class FileService {
        
        private static string filePath = "save/entities";

        public static List<EntityJson> loadEntityJson() {
            var file = Resources.Load<TextAsset>("save/entities");
            var fileData = file.text;
            var entityJson = JsonConvert.DeserializeObject<List<EntityJson>>(fileData);
            return entityJson;
        }

        public static void SaveEntities(IEnumerable<Entity> entities) {
            var data = new List<EntityJson>();
            foreach (var entity in entities) {
                var entityPosition = entity.transform.position;
                var entityObject = new EntityJson() {
                    name = entity.config.configName,
                    position = new Position(entityPosition.x, entityPosition.y)
                };
                data.Add(entityObject);
            }
            var entitiesString = JsonConvert.SerializeObject(data);

            var fileStream = new FileStream("Assets/Resources/" + filePath + ".json", FileMode.Create);

            using var writer = new StreamWriter(fileStream);
            writer.Write(entitiesString);
        }
    }

    
    
    public struct EntityJson {
        public string name;
        public Position position;
    }

    public struct Position {
        public float x;
        public float y;

        public Position(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public Vector2 GetV2() {
            return new Vector2(x, y);
        }
    }
}