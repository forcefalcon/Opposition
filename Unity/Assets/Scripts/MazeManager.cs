using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class MazeManager : MonoBehaviour
{
	public static MazeManager Instance { get; private set; }

	public GameObject MetalRoomPrefab;
	public GameObject WoodRoomPrefab;
	public GameObject ConcreteRoomPrefab;
	public GameObject WaterRoomPrefab;
	public GameObject DirtRoomPrefab;
	
	public GameObject SpikesTrapPrefab;
	public GameObject FlameThrowerTrapPrefab;
	public GameObject LiquidThrowerTrapPrefab;
	public GameObject ProjectilesTrapPrefab;
		
	public GameObject PlayerPrefab;
	public List<int> GoalRoomIDs;
	
	private Dictionary<int, RoomController> Rooms { get; set; }
//	private Dictionary<int, Trap> Traps { get; set; }

	public void Awake() {
		Instance = this;
		Rooms = new Dictionary<int, RoomController> ();
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
		GoalRoomIDs = null;
		
		if (!ValidateMaze(mazeInfo))
		{
			return;
		}
		
		GoalRoomIDs = mazeInfo.GoalRoomIDs;
			
		foreach (Serialization.RoomInfo roomInfo in mazeInfo.Rooms) {
			var roomPrefab = SelectRoomPrefab(roomInfo.FloorMaterial);
			var instance = (GameObject)GameObject.Instantiate(
				roomPrefab, 
				new Vector3(
					RoomCoordToVectorComponent(roomInfo.Position.X),
					0f,
					RoomCoordToVectorComponent(roomInfo.Position.Y)),
				Quaternion.identity);
			
			var room = instance.GetComponent<RoomController>();
			room.Connections = roomInfo.Connections;
			
			instance.transform.parent = this.transform;	
			Rooms[roomInfo.ID] = room;
		}
		
		//foreach (Serialization.TrapInfo trapInfo in mazeInfo.Traps) {
		//	GameObject trapPrefab = SelectTrapPrefab(trapInfo.Type);
		//}
		
		// Spawn character
		var startingRoomPos = Rooms[mazeInfo.StartingRoomID].transform.position;
		var player = (GameObject)GameObject.Instantiate(PlayerPrefab);
		player.transform.position = startingRoomPos;
		player.transform.Rotate(new Vector3(0f, mazeInfo.StartingDirection.GetRotation(), 0f));
		player.name = "MazePlayerController";
	}
	
	private GameObject SelectRoomPrefab(RoomMaterialType materialType)
	{
		switch (materialType)
		{
			case RoomMaterialType.Metal:
				return MetalRoomPrefab;
			case RoomMaterialType.Wood:
				return WoodRoomPrefab;
			case RoomMaterialType.Concrete:
				return ConcreteRoomPrefab;
			case RoomMaterialType.Dirt:
				return DirtRoomPrefab;
			case RoomMaterialType.Water:
				return WaterRoomPrefab;
		}
		Debug.LogWarning("Unknown floor material type " + materialType + ", defaulting to Metal");
		return MetalRoomPrefab;
	}
	
	private GameObject SelectTrapPrefab(TrapType trapType)
	{
		switch (trapType)
		{
			case TrapType.Spikes:
				return SpikesTrapPrefab;
			case TrapType.FlameThrower:
				return FlameThrowerTrapPrefab;
			case TrapType.LiquidThrower:
				return LiquidThrowerTrapPrefab;
			case TrapType.Projectiles:
				return ProjectilesTrapPrefab;
		}
		Debug.LogWarning("Unknown trap type " + trapType + ", defaulting to Spikes");
		return SpikesTrapPrefab;
	}

	private static bool ValidateMaze(Serialization.MazeInfo mazeInfo){
		if (mazeInfo.Rooms == null || mazeInfo.Rooms.Count == 0)
		{
			Debug.LogError ("Cannot load maze - no rooms defined");
			return false;
		}
		List<string> errors = new List<string>();
		
		var roomIDs = new HashSet<int>();
		// Ensure no two rooms share the same ID
		// Ensure no two rooms occupy the same space
		var roomsByLocation = new Dictionary<Serialization.IntVector2, Serialization.RoomInfo>();
		foreach (var room in mazeInfo.Rooms)
		{
			if (roomIDs.Contains (room.ID))
			{
				errors.Add ("Duplicate Room ID " + room.ID);
			}
			else
			{
				roomIDs.Add (room.ID);
			}
			Serialization.RoomInfo existingRoom;
			if (roomsByLocation.TryGetValue (room.Position, out existingRoom))
			{
				errors.Add ("Two rooms defined for location " + room.Position.ToString () + ": IDs " + room.ID + " and " + existingRoom.ID);
			}
			roomsByLocation[room.Position] = room;
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
				if (!roomsByLocation.TryGetValue(otherLocation, out otherRoom))
				{
					errors.Add ("Room with ID " + room.ID + " has no neighbor " + direction.ToString() + " of it");
				}
				else if (!otherRoom.Connections.Contains (expectedDirection))
				{
					errors.Add ("Room with ID " + otherRoom.ID + " does not reciprocate a Connection defined in Room with ID " + room.ID);
				}
			}
		}
		
		var trapIDs = new HashSet<int>();
		var trapsByKeyBinding = new Dictionary<KeyCode, int>();
		var trapPlacements = new HashSet<int>();
		// Ensure traps are defined in existing rooms
		foreach (var trap in mazeInfo.Traps)
		{
			if (trapIDs.Contains (trap.ID))
			{
				errors.Add ("Duplicate Trap ID " + trap.ID);
			}
			else
			{
				trapIDs.Add (trap.ID);
			}
			
			if (trapsByKeyBinding.ContainsKey(trap.KeyBinding))
			{
				errors.Add (trap.KeyBinding + " key is already bound to Trap with ID " + trapsByKeyBinding[trap.KeyBinding]);
			}
			else
			{
				trapsByKeyBinding[trap.KeyBinding] = trap.ID;
			}
			
			if (!roomIDs.Contains (trap.RoomID))
			{
				errors.Add ("Trap with ID " + trap.ID + " references non-existent Room ID " + trap.RoomID);
			}
			
			// combine the room ID and placement enum in a single int value for easily spotting dupes
			var placement = (trap.RoomID << 4) | (((int)trap.Placement) & 0xF); 
			if (trapPlacements.Contains(placement))
			{
			Debug.Log (placement);
				errors.Add ("The placement of Trap with ID " + trap.ID + " is invalid because Room with ID " + trap.RoomID + " already contains a Trap in its " + trap.Placement.ToString() + " section.");
			}
			else
			{
				Debug.Log (placement);
				trapPlacements.Add (placement);
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