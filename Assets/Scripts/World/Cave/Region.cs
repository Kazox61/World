using System.Collections.Generic;

namespace WorldNS {
	public class Region {
		public List<Coord> tiles;
		public int lowestX;
		public int lowestY;
		public int heighestX;
		public int heighestY;


		public Region() { }

		public Region(List<Coord> roomTiles) {
			tiles = roomTiles;
		}

		public static List<Region> GetRegions(int tileType, int[,] map, int width, int height) {
			List<Region> regions = new List<Region>();
			int[,] mapFlags = new int[width, height];

			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					if (mapFlags[x, y] == 0 && map[x, y] == tileType) {
						Region newRegion = GetRegionTiles(x, y, map, width, height);
						regions.Add(newRegion);

						foreach (var tile in newRegion.tiles) {
							mapFlags[tile.tileX, tile.tileY] = 1;
						}
					}
				}
			}

			return regions;
		}

		private static Region GetRegionTiles(int startX, int startY, int[,] map, int width, int height) {
			int smallestX = startX;
			int largestX = startX;
			int smallestY = startY;
			int largestY = startY;
			List<Coord> tiles = new List<Coord>();
			int[,] mapFlags = new int[width, height];
			int tileType = map[startX, startY];

			Queue<Coord> queue = new Queue<Coord>();
			queue.Enqueue(new Coord(startX, startY));
			mapFlags[startX, startY] = 1;

			while (queue.Count > 0) {
				Coord tile = queue.Dequeue();
				tiles.Add(tile);
				if (tile.tileX > largestX) {
					largestX = tile.tileX;
				}

				if (tile.tileX < smallestX) {
					smallestX = tile.tileX;
				}

				if (tile.tileY > largestY) {
					largestX = tile.tileX;
				}

				if (tile.tileY < smallestY) {
					smallestY = tile.tileY;
				}


				for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
					for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
						if (IsInMapRange(x, y, width, height) && (y == tile.tileY || x == tile.tileX)) {
							if (mapFlags[x, y] == 0 && map[x, y] == tileType) {
								mapFlags[x, y] = 1;
								queue.Enqueue(new Coord(x, y));
							}
						}
					}
				}
			}

			var region = new Region(tiles);
			region.lowestX = smallestX;
			region.heighestX = largestX;
			region.lowestY = smallestY;
			region.heighestY = largestY;
			return region;
		}

		protected static bool IsInMapRange(int x, int y, int width, int height) {
			return (x >= 0 && x < width && y >= 0 && y < height);
		}
	}
}