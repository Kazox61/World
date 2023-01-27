using System.Collections.Generic;
using System.Linq;
using GameNS.Config;
using GameNS.Entity;
using ServiceNS;
using UnityEngine;

namespace WorldNS {
    public class FieldController : MonoBehaviour {
        public static FieldController Instance { get; private set; }
        
        public List<Tile> occupiedTiles = new List<Tile>();

        public void Awake() {
            Instance = this;
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
            
            var fieldPos = ChunkHelper.PositionToFieldPosition(centerPoint); 
            var chunkPosition = ChunkHelper.FieldToChunkPosition(fieldPos);
            ChunkManager.Instance.AddEntityToChunk(chunkPosition, entity);
            
            entity.config = entityConfig;
            var area = new Area(entity);

            for (int y = 0; y < sizeY; y++) {
                for (int x = 0; x < sizeX; x++) {
                    var point = new Vector2(startPoint.x + x, startPoint.y + y);
                    var tile = new Tile(point, area);
                    occupiedTiles.Add(tile);
                }
            }

            return entity;
        }

        public bool TryGetEntity(Vector2 position, out Entity entity) {
            entity = null;
            var floor = Vector2Int.FloorToInt(position);

            var tile = occupiedTiles.FirstOrDefault(match => match.position.Equals(floor));
            if (tile == null) {
                return false;
            }

            entity = tile.area.entity;
            return true;
        }

        public void RemoveEntity(Entity entity) {
            var tiles = occupiedTiles.Where(tile => tile.area.entity == entity);

            var fieldPos = ChunkHelper.PositionToFieldPosition(entity.transform.position);
            var chunkPosition = ChunkHelper.FieldToChunkPosition(fieldPos);
            ChunkManager.Instance.RemoveEntityFromChunk(chunkPosition, entity);
            
            occupiedTiles.RemoveAll(tile => tiles.Contains(tile));
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