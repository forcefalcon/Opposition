using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class MazeManager : MonoBehaviour
{
	public static MazeManager Instance { get; private set; }

	public GameObject MetalRoomPrefab;

	public int StartingRoomID;
	public List<int> GoalRoomIDs;
	
	private Dictionary<int, Room> Rooms { get; set; }
	private Dictionary<int, Trap> Traps { get; set; }

	public void Awake() {
		Instance = this;
		Rooms = new Dictionary<int, Room> ();
	}
		
	public void Start(){

		// testing
		Serialization.MazeInfo mazeInfo = null;
		using (var reader = new StreamReader(@"Assets\Data\Sample.maze")) {
			string json = reader.ReadToEnd();
			mazeInfo = JsonConvert.DeserializeObject<Serialization.MazeInfo>(json);
		}
		if (mazeInfo != null) {
			CreateMaze (mazeInfo);
		} else {
			Debug.LogError ("No MazeInfo found");
		}
	}

	public void CreateMaze(Serialization.MazeInfo mazeInfo) {
		ClearRooms ();
		StartingRoomID = 0;
		GoalRoomIDs = null;
		
		if (!ValidateMaze(mazeInfo))
		{
			return;
		}
		
		StartingRoomID = mazeInfo.StartingRoomID;
		GoalRoomIDs = mazeInfo.GoalRoomIDs;
			
		foreach (Serialization.RoomInfo roomInfo in mazeInfo.Rooms) {
			// TODO validate roomInfo and Debug.Log errors
			// (e.g. invalid connections, overlapping rooms)

			// TODO switch on roomInfo.FloorMaterial to get the right prefab
			var roomPrefab = MetalRoomPrefab;
			var instance = (GameObject)GameObject.Instantiate(
				roomPrefab, 
				new Vector3(
					RoomCoordToVectorComponent(roomInfo.Position.X),
					0f,
					RoomCoordToVectorComponent(roomInfo.Position.Y)),
				Quaternion.identity);
			
			var room = instance.GetComponent<Room>();
			room.Connections = roomInfo.Connections;
			
			instance.transform.parent = this.transform;	
			Rooms[roomInfo.ID] = room;
		}
		
		foreach (Serialization.TrapInfo trapInfo in mazeInfo.Traps) {
		}
		//var trap = instance.GetComponent<Trap>();
		//room.Connections = roomInfo.Connections;
	}

	private static bool ValidateMaze(Serialization.MazeInfo mazeInfo){
		if (mazeInfo.Rooms == null || mazeInfo.Rooms.Count == 0)
		{
			Debug.LogError ("Cannot load maze - no rooms defined");
			return false;
		}
		List<string> errors = new List<string>();
		
		// Ensure no two rooms occupy the same space
		var roomsPerLocation = new Dictionary<Serialization.IntVector2, Serialization.RoomInfo>();
		foreach (var room in mazeInfo.Rooms)
		{
			Serialization.RoomInfo existingRoom;
			if (roomsPerLocation.TryGetValue (room.Position, out existingRoom))
			{
				errors.Add ("Two rooms defined for location " + room.Position.ToString () + ": IDs " + room.ID + " and " + existingRoom.ID);
			}
			roomsPerLocation[room.Position] = room;
		}
		// Ensure all rooms connect properly
		foreach (var room in mazeInfo.Rooms)
		{
			foreach (var direction in room.Connections)
			{
				Serialization.IntVector2 otherLocation = room.Position;
				Direction expectedDirection;
				switch (direction)
				{
					case Direction.North:
						{
							otherLocation = new Serialization.IntVector2(room.Position.X, room.Position.Y + 1);
							expectedDirection = Direction.South;
						} break;
					case Direction.South:
						{
							otherLocation = new Serialization.IntVector2(room.Position.X, room.Position.Y - 1);
							expectedDirection = Direction.North;
						} break;
					case Direction.West:
						{
							otherLocation = new Serialization.IntVector2(room.Position.X - 1, room.Position.Y);
							expectedDirection = Direction.East;
						} break;
					case Direction.East:
						{
							otherLocation = new Serialization.IntVector2(room.Position.X + 1, room.Position.Y);
							expectedDirection = Direction.West;
						} break;
					default: 
						continue;
				}
				Serialization.RoomInfo otherRoom;
				if (!roomsPerLocation.TryGetValue(otherLocation, out otherRoom))
				{
					errors.Add ("Room with ID " + room.ID + " has no neighbor " + direction.ToString() + " of it");
				}
				else if (!otherRoom.Connections.Contains (expectedDirection))
				{
					errors.Add ("Room with ID " + otherRoom.ID + " does not reciprocate a Connection defined in Room with ID " + room.ID);
				}
			}
		}
		
		if (errors.Count > 0)
		{
			foreach (var error in errors)
			{
				Debug.LogError(error);
			}
			return false;
		}
		return true;
	}

	private float RoomCoordToVectorComponent(float coord)
	{
		return coord * (RoomConstants.RoomSize + RoomConstants.RoomSpacing); 
	}

	private void ClearRooms() {
		foreach (var room in Rooms.Values) {
			GameObject.Destroy(room.gameObject);
		}
		Rooms.Clear ();
	}
}