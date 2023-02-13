using System.Collections;
using System.Collections.Generic;
using SaveSystemNS;
using ServiceNS;
using SetupNS;
using UnityEngine;

namespace WorldNS {
	[System.Serializable]
	public class Chunk : PoolableObject {
		public Vector2Int position;
		public int Size => ChunkManager.CHUNK_SIZE;
		public Vector2Int originField; // lower left field in this chunk
		public Rect rect;
		public FieldController[] fieldControllers;

		private float keepaliveTime;

		public ChunkTransformer chunkTransformer;

		public bool constructed;


		public static Chunk CreateChunk(Vector2Int position) {
			var chunk = ObjectPool<Chunk>.Get();
			chunk.Initialize(position);
			chunk.Construct();
			return chunk;
		}

		public void Construct() {
			if (constructed) {
				return;
			}

			Constructor();
			constructed = true;
		}

		public void Constructor() {
			chunkTransformer = new ChunkTransformer(this);
			fieldControllers = new FieldController[Size * Size];

			for (int y = 0; y < Size; y++) {
				for (int x = 0; x < Size; x++) {
					var index = x + Size * y;
					var field = originField + new Vector2Int(x, y);
					var fieldController = FieldController.Create(field);
					fieldControllers[index] = fieldController;
				}
			}
		}

		public void Initialize(Vector2Int position) {
			this.position = position;
			originField = position * Size;
			rect = new Rect(originField.x, originField.y, Size, Size);

			Keepalive();
		}

		public void Keepalive() {
			keepaliveTime = 5;
		}

		public bool UpdateKeepalive() {
			keepaliveTime -= Time.deltaTime;
			return keepaliveTime <= 0;
		}
	}
}