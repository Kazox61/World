using GameNS;
using ServiceNS;
using UnityEngine;

namespace WorldNS {
    public class TerrainController : MonoBehaviour {
        
        private void Awake() {
        }

        private void Start() {
            Delay.Start(() => {
                var setup = Entity.SetupCollection.GetSetup("House0");
                var entity = Entity.Create(setup, Vector3.zero);
                var chunkPosition = ChunkHelper.FieldToChunkPosition(Vector2Int.zero);
                
                var success = ChunkManager.Instance.TryGetChunk(chunkPosition, out var chunk);
                
                chunk.entities.Add(entity);
                Debug.Log(Entity.CanCreateEntity(setup, Vector2Int.up));
            }, 2);
        }

    }
}