using System;
using System.Collections;
using System.Collections.Generic;
using SetupNS;
using UnityEngine;

namespace WorldNS {
	public class CaveGeneration : MonoBehaviour {
		public int width;
		public int height;

		public string seed;
		public bool useRandomSeed;
		public int smoothIterationAmount = 5;

		public int wallThresholdSize = 50;
		public int roomThresholdSize = 50;

		public int passageRadius = 3;

		[Range(0, 100)] public int randomFillPercent;

		int[,] map;

		public void Start() {
			var field = GridHelper.PositionToField(transform.position);
			ChunkManager.Instance.UpdateChunks(field);
			GenerateMap();
			StartCoroutine(Create());
		}

		public IEnumerator Create() {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					if (map[x, y] == 1) {
						Entity.CreateEntity(SetupCore.GetEntitySetup("StoneWall"), new Vector2Int(x, y));
						yield return null;
					}
				}
			}
		}

		private void GenerateMap() {
			map = new int[width, height];
			RandomFillMap();

			for (int i = 0; i < smoothIterationAmount; i++) {
				SmoothMap();
			}

			ProcessMap();
		}

		void ProcessMap() {
			Unsmooth(7, 7);
			List<Region> wallRegions = Region.GetRegions(1, map, width, height);

			foreach (var wallRegion in wallRegions) {
				if (wallRegion.tiles.Count < wallThresholdSize) {
					foreach (Coord tile in wallRegion.tiles) {
						map[tile.tileX, tile.tileY] = 0;
					}
				}
			}

			List<Region> roomRegions = Region.GetRegions(0, map, width, height);

			List<Room> survivingRooms = new List<Room>();

			foreach (Region roomRegion in roomRegions) {
				if (roomRegion.tiles.Count < roomThresholdSize) {
					foreach (Coord tile in roomRegion.tiles) {
						map[tile.tileX, tile.tileY] = 1;
					}
				}
				else {
					survivingRooms.Add(new Room(roomRegion.tiles, map, width, height));
				}
			}

			survivingRooms.Sort();
			survivingRooms[0].isMainRoom = true;
			survivingRooms[0].isAccessibleFromMainRoom = true;
			ConnectClosestRooms(survivingRooms);
			Unsmooth(6, 6);
		}

		void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false) {
			List<Room> roomListA = new List<Room>();
			List<Room> roomListB = new List<Room>();

			if (forceAccessibilityFromMainRoom) {
				foreach (var room in allRooms) {
					if (room.isAccessibleFromMainRoom) {
						roomListB.Add(room);
					}
					else {
						roomListA.Add(room);
					}
				}
			}
			else {
				roomListA = allRooms;
				roomListB = allRooms;
			}

			int bestDinstance = 0;
			Coord bestTileA = new Coord();
			Coord bestTileB = new Coord();
			Room bestRoomA = new Room();
			Room bestRoomB = new Room();
			bool possibleConnectionFound = false;

			foreach (var roomA in roomListA) {
				if (!forceAccessibilityFromMainRoom) {
					possibleConnectionFound = false;
					if (roomA.connectedRooms.Count > 0) {
						continue;
					}
				}

				foreach (var roomB in roomListB) {
					if (roomA == roomB || roomA.IsConnected(roomB)) continue;


					for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++) {
						for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++) {
							Coord tileA = roomA.edgeTiles[tileIndexA];
							Coord tileB = roomB.edgeTiles[tileIndexB];
							int distanceBetweenRooms = (int)(MathF.Pow(tileA.tileX - tileB.tileX, 2) +
							                                 MathF.Pow(tileA.tileY - tileB.tileY, 2));

							if (distanceBetweenRooms < bestDinstance || !possibleConnectionFound) {
								bestDinstance = distanceBetweenRooms;
								possibleConnectionFound = true;
								bestTileA = tileA;
								bestTileB = tileB;
								bestRoomA = roomA;
								bestRoomB = roomB;
							}
						}
					}
				}

				if (possibleConnectionFound && !forceAccessibilityFromMainRoom) {
					CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
				}
			}

			if (possibleConnectionFound && forceAccessibilityFromMainRoom) {
				CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
				ConnectClosestRooms(allRooms, true);
			}

			if (!forceAccessibilityFromMainRoom) {
				ConnectClosestRooms(allRooms, true);
			}
		}

		private void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB) {
			Room.ConnectRooms(roomA, roomB);

			var line = GetLine(tileA, tileB);
			foreach (var c in line) {
				DrawCircle(c, passageRadius);
			}
		}

		void DrawCircle(Coord c, int r) {
			for (int x = -r; x <= r; x++) {
				for (int y = -r; y <= r; y++) {
					if (x * x + y * y <= r * r) {
						int drawX = c.tileX + x;
						int drawY = c.tileY + y;
						if (IsInMapRange(drawX, drawY)) {
							map[drawX, drawY] = 0;
						}
					}
				}
			}
		}

		private List<Coord> GetLine(Coord from, Coord to) {
			List<Coord> line = new List<Coord>();

			int x = from.tileX;
			int y = from.tileY;

			int dx = to.tileX - from.tileX;
			int dy = to.tileY - from.tileY;

			bool inverted = false;
			int step = Math.Sign(dx);
			int gradientStep = Math.Sign(dy);

			int longest = Mathf.Abs(dx);
			int shortest = Mathf.Abs(dy);

			if (longest < shortest) {
				inverted = true;
				longest = Mathf.Abs(dy);
				shortest = Mathf.Abs(dx);

				step = Math.Sign(dy);
				gradientStep = Math.Sign(dx);
			}

			int gradientAccumulation = longest / 2;
			for (int i = 0; i < longest; i++) {
				line.Add(new Coord(x, y));

				if (inverted) {
					y += step;
				}
				else {
					x += step;
				}

				gradientAccumulation += shortest;

				if (gradientAccumulation >= longest) {
					if (inverted) {
						x += gradientStep;
					}
					else {
						y += gradientStep;
					}

					gradientAccumulation -= longest;
				}
			}

			return line;
		}


		private void Unsmooth(int xSmooth, int ySmooth) {
			for (int x = 3; x < width; x++) {
				var rowCount = 0;
				var removingTiles = new List<Coord>();
				for (int y = 3; y < height; y++) {
					if (map[x, y] == 1) {
						var checkCoord = new Coord(x, y);
						removingTiles.Add(checkCoord);
						rowCount++;
					}
					else {
						if (rowCount < xSmooth) {
							foreach (var tile in removingTiles) {
								map[tile.tileX, tile.tileY] = 0;
							}
						}

						rowCount = 0;
						removingTiles.Clear();
					}
				}
			}

			for (int y = 3; y < height; y++) {
				var rowCount = 0;
				var removingTiles = new List<Coord>();
				for (int x = 3; x < width; x++) {
					if (map[x, y] == 1) {
						var checkCoord = new Coord(x, y);
						removingTiles.Add(checkCoord);
						rowCount++;
					}
					else {
						if (rowCount < ySmooth) {
							foreach (var tile in removingTiles) {
								map[tile.tileX, tile.tileY] = 0;
							}
						}

						removingTiles.Clear();
						rowCount = 0;
					}
				}
			}
		}

		public bool IsInMapRange(int x, int y) {
			return (x >= 0 && x < width && y >= 0 && y < height);
		}

		private void RandomFillMap() {
			if (useRandomSeed) {
				seed = Time.time.ToString();
			}

			System.Random pseudoRandom = new System.Random(seed.GetHashCode());

			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					if (x < 10 || x > width - 10 || y < 10 || y > height - 10) {
						map[x, y] = 1;
					}
					else {
						map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
					}
				}
			}
		}

		private void SmoothMap() {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					int neighbourWallTiles = GetSurroundingWallCount(x, y);

					if (neighbourWallTiles > 4) {
						map[x, y] = 1;
					}
					else if (neighbourWallTiles < 4) {
						map[x, y] = 0;
					}
				}
			}
		}

		int GetSurroundingWallCount(int gridX, int gridY) {
			int wallCount = 0;
			for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
				for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
					if (IsInMapRange(neighbourX, neighbourY)) {
						if (neighbourX != gridX || neighbourY != gridY) {
							wallCount += map[neighbourX, neighbourY];
						}
					}
					else {
						wallCount++;
					}
				}
			}

			return wallCount;
		}
	}

	public struct Coord {
		public int tileX;
		public int tileY;

		public Coord(int x, int y) {
			tileX = x;
			tileY = y;
		}

		public Vector3 CoordToWorldPoint() {
			return new Vector3(tileX, tileY);
		}
	}
}