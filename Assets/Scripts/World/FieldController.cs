using System.Collections.Generic;
using System.Linq;
using GameNS.Config;
using GameNS.Entity;
using ServiceNS;
using UnityEngine;

namespace WorldNS {
    public class FieldController : MonoBehaviour {
        public static FieldController Instance { get; private set; }
        
        public List<Field> occupiedFields = new List<Field>();
        public List<Entity> entities = new List<Entity>();

        public void Awake() {
            Instance = this;
        }

        public void Start() {
            LoadEntitiesFromFile();
        }

        private void LoadEntitiesFromFile() {
            var json = FileService.loadEntityJson();
            foreach (var entityJson in json) {
                TryCreateEntity(EntityConfigCore.GetConfig(entityJson.name), entityJson.position.GetV2(), out _);
            }
        }

        public bool TryCreateEntity(EntityConfig entityConfig, Vector2 position, out Entity entity) {
            entity = null;
            var exist = TryGetEntity(position, out entity);
            if (exist) {
                return false;
            }
            
            var sizeX = entityConfig.sizeX;
            var sizeY = entityConfig.sizeY;

            var centerPoint = entityConfig.GetAreaCenter(position);
            var startPoint = new Vector2(centerPoint.x - sizeX * 0.5f, centerPoint.y - sizeY * 0.5f);

            var isAreaBlocked = IsAreaBlocked(centerPoint, sizeX, sizeY);
            if (isAreaBlocked) {
                return false;
            }

            entity = CreateEntity(entityConfig, centerPoint, startPoint, sizeX, sizeY);

            UpdateNeighbors(position);

            return true;
        }

        private Entity CreateEntity(EntityConfig entityConfig, Vector2 centerPoint, Vector2 startPoint, int sizeX, int sizeY) {
            var entity = Entity.Create(entityConfig, centerPoint);

            entities.Add(entity);
            entity.config = entityConfig;
            var area = new Area(entity);

            for (int y = 0; y < sizeY; y++) {
                for (int x = 0; x < sizeX; x++) {
                    var point = new Vector2(startPoint.x + x, startPoint.y + y);
                    var field = new Field(point, area);
                    occupiedFields.Add(field);
                }
            }

            return entity;
        }

        public bool TryGetEntity(Vector2 position, out Entity entity) {
            entity = null;
            var floor = Vector2Int.FloorToInt(position);

            var field = occupiedFields.FirstOrDefault(field => field.position.Equals(floor));
            if (field == null) {
                return false;
            }

            entity = field.area.entity;
            return true;
        }

        public void RemoveEntity(Entity entity) {
            var fields = occupiedFields.Where(field => field.area.entity == entity);
            entities.Remove(entity);
            occupiedFields.RemoveAll(field => fields.Contains(field));
            entity.ReleaseToPool();
        }

        public bool TryRemoveEntity(Vector2 position) {
            var found = TryGetEntity(position, out var entity);
            if (!found) {
                
                
                return false;
            }
            RemoveEntity(entity);
            
            UpdateNeighbors(position);
            return true;
        }

        public bool IsAreaBlocked(Vector2 centerPoint, int sizeX, int sizeY) {
            var startPoint = new Vector2(centerPoint.x - sizeX * 0.5f, centerPoint.y - sizeY * 0.5f);
            
            for (int y = 0; y < sizeY; y++) {
                for (int x = 0; x < sizeX; x++) {
                    var point = new Vector2(startPoint.x + x, startPoint.y + y);
                    var blocked = TryGetEntity(point, out var entity);
                    if (blocked) {
                        return true;
                    }
                }
            }
            return false;
        }
        
        private void UpdateNeighbors(Vector2 position) {
            for (int y = -1; y <= 1; y++) {
                for (int x = -1; x <= 1; x++) {
                    var offset = new Vector2(x, y);
                    var found = TryGetEntity(position + offset, out var entity);
                    if (found) {
                        entity.UpdateSprite();
                    }
                }
            }
        }
    }
}