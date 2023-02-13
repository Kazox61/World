using System;
using System.Collections.Generic;

namespace WorldNS {
	public class Room : Region, IComparable<Room> {
		public List<Coord> edgeTiles;
		public List<Room> connectedRooms;
		public int roomSize;
		public bool isAccessibleFromMainRoom;
		public bool isMainRoom;

		public Room() { }

		public Room(List<Coord> roomTiles, int[,] map, int width, int height) : base(roomTiles) {
			tiles = roomTiles;
			roomSize = tiles.Count;
			connectedRooms = new List<Room>();

			edgeTiles = new List<Coord>();
			foreach (var tile in tiles) {
				for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
					for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
						if ((x == tile.tileX || y == tile.tileY) && IsInMapRange(x, y, width, height)) {
							if (map[x, y] == 1) {
								edgeTiles.Add(tile);
							}
						}
					}
				}
			}
		}

		public void SetAccessibleFromRoom() {
			if (!isAccessibleFromMainRoom) {
				isAccessibleFromMainRoom = true;
				foreach (Room connectedRoom in connectedRooms) {
					connectedRoom.SetAccessibleFromRoom();
				}
			}
		}

		public static void ConnectRooms(Room roomA, Room roomB) {
			if (roomA.isAccessibleFromMainRoom) {
				roomB.SetAccessibleFromRoom();
			}
			else if (roomB.isAccessibleFromMainRoom) {
				roomA.SetAccessibleFromRoom();
			}

			roomA.connectedRooms.Add(roomB);
			roomB.connectedRooms.Add(roomA);
		}

		public int CompareTo(Room otherRoom) {
			return otherRoom.roomSize.CompareTo(roomSize);
		}

		public bool IsConnected(Room otherRoom) {
			return connectedRooms.Contains(otherRoom);
		}
	}
}