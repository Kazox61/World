using System;
using System.Collections.Generic;
using GameNS.Config;
using ServiceNS;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldNS {
    public class TerrainController : MonoBehaviour {
        public static TerrainController Instance { get; private set; }
        
        public Tilemap[] layers;
        
        private void Awake() {
            Instance = this;
        }

        public bool TryPlaceField(Vector2 position, Field field) {
            var fieldPosition = ChunkHelper.PositionToFieldPosition(position);
            
            ChunkManager.Instance.AddFieldToChunk(fieldPosition, field);
            
            var terrainConfig = TerrainConfigCore.GetConfig(string.IsNullOrEmpty(field.terrain) ? "Dirt" : field.terrain);
            field.terrain = terrainConfig.configName;
            layers[(int)terrainConfig.layer].SetTile((Vector3Int)fieldPosition, terrainConfig);
            return true;
        }

        public bool TryRemoveField(Vector2 position, Field field) {
            var fieldPosition = ChunkHelper.PositionToFieldPosition(position);
            ChunkManager.Instance.RemoveFieldFromChunk(fieldPosition);
            
            var terrainConfig = TerrainConfigCore.GetConfig(string.IsNullOrEmpty(field.terrain) ? "Dirt" : field.terrain);
            layers[(int)terrainConfig.layer].SetTile((Vector3Int)fieldPosition, null);
            return true;
        }
    }
}