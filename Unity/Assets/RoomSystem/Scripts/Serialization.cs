using UnityEngine;
using System.Collections.Generic;

namespace Serialization
{
	public class IntVector2
	{
		public int X;
		public int Y;
	}

	public class RoomInfo
	{
		public int ID;
		public IntVector2 Position;
		public RoomMaterialType FloorMaterial;
		public List<Direction> Connections;
	}

	public class MazeInfo {
		public int StartingRoomID;
		public List<int> GoalRoomIDs;
		public List<RoomInfo> Rooms;
	}
}