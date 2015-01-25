using UnityEngine;
using System.Collections.Generic;

namespace Serialization
{
	public struct IntVector2
	{
		public int X;
		public int Y;
		
		public IntVector2(int x, int y)
		{
			X = x;
			Y = y;
		}
		
		public override string ToString ()
		{
			return string.Format ("({0},{1}", X, Y);
		}
	}

	public class RoomInfo
	{
		public int ID;
		public IntVector2 Position;
		public RoomMaterialType FloorMaterial;
		public List<Direction> Connections;
	}
	
	public class TrapInfo
	{
		public int ID;
		public int RoomID;
		public TrapType Type;
		public Placement Placement;
		public Direction Direction;
		public KeyCode KeyBinding;
	}

	public class MazeInfo {
		public int StartingRoomID;
		public List<int> GoalRoomIDs;
		public List<RoomInfo> Rooms;
		public List<TrapInfo> Traps;
	}
}