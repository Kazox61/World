using System;
using System.IO;
using Newtonsoft.Json;
using ServiceNS;
using UnityEngine;
using WorldNS;

namespace SaveSystemNS {
	public class SaveSystem {
		public static SaveSystem Instance = new();
		private World world;

		public World World {
			get {
				if (world == null) {
					LoadWorld();
				}

				return world;
			}
		}

		private void LoadWorld() {
			var file = Resources.Load<TextAsset>("save/world");
			if (file == null) {
				world = new World();
				return;
			}

			var fileData = file.text;
			world = JsonConvert.DeserializeObject<World>(fileData);
		}

		public void Save(DataChunk[] dataChunks) {
			world.chunks = dataChunks;
			var dataString = JsonConvert.SerializeObject(world);
			var fileStream = new FileStream("Assets/Resources/save/world.json", FileMode.Create);
			using var writer = new StreamWriter(fileStream);
			writer.Write(dataString);
		}

		public void Build() {
			foreach (var chunkData in world.chunks) {
				var chunk = ObjectPool<Chunk>.Get();
				chunk.chunkTransformer.FromData(chunkData);
			}
		}
	}
}