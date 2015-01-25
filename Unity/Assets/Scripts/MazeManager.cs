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
	
	public GameObject TrapGroupPrefab; // Enables multiple instances of the same trap prefab controlled by the same controller
	
	public GameObject PlayerPrefab;
	public bool UseOVR;
	public List<int> GoalRoomIDs;
	
	private Dictionary<int, GameObject> Rooms = new Dictionary<int, GameObject>();

	public void Awake() {
		Instance = this;
	}
		
	public void Start(){
		// testing
		Serialization.MazeInfo mazeInfo = null;
		using (var reader = new StreamReader(Application.streamingAssetsPath + "/Data/Sample.maze")) {
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
		Clear ();
		GoalRoomIDs = null;
		
		if (!ValidateMaze(mazeInfo))
		{
			return;
		}
		
		GoalRoomIDs = mazeInfo.GoalRoomIDs;
		
		// Spawn rooms
		foreach (Serialization.RoomInfo roomInfo in mazeInfo.Rooms) {
			var roomPrefab = SelectRoomPrefab(roomInfo.FloorMaterial);
			var roomGO = (GameObject)GameObject.Instantiate(
				roomPrefab, 
				new Vector3(
					RoomCoordToVectorComponent(roomInfo.Position.X),
					0f,
					RoomCoordToVectorComponent(roomInfo.Position.Y)),
				Quaternion.identity);
			
			roomGO.transform.parent = this.transform;
			Rooms[roomInfo.ID] = roomGO;
			
			var room = roomGO.GetComponent<RoomController>();
			room.Connections = roomInfo.Connections;
		}
		
		// spawn traps
		foreach (Serialization.TrapInfo trapInfo in mazeInfo.Traps) {
			var trapGroupGO = (GameObject)GameObject.Instantiate(
				TrapGroupPrefab, 
				new Vector3(
					0f /* TODO offset based on Placement */,
					0f,
					0f /* TODO offset based on Placement */),
				Quaternion.identity);
			
			var trapRoomGO = Rooms[trapInfo.RoomID];
			var trapsChild = trapRoomGO.transform.Find("Traps");
			if (trapsChild == null)
			{
				var trapsChildGO = new GameObject();
				trapsChild = trapsChildGO.transform;
				trapsChild.name = "Traps";
				trapsChild.parent = trapRoomGO.transform;
			}
			trapGroupGO.transform.parent = trapsChild;
			trapGroupGO.name = trapInfo.Type.ToString();
			
			trapGroupGO.AddComponent(SelectTrapController(trapInfo.Type));
			
			var trapController = trapGroupGO.GetComponent<TrapController>();
			if (trapController != null)
			{
				TrapManager.Instance.RegisterTrap(trapController, trapInfo.KeyBinding);
				trapController.Placement = trapInfo.Placement;
				trapController.Direction = trapInfo.Direction;
			}
		}
		
		// Spawn character
		var startingRoomPos = Rooms[mazeInfo.StartingRoomID].transform.position;
		var player = (GameObject)GameObject.Instantiate(PlayerPrefab);
		player.transform.position = startingRoomPos;
		player.transform.Rotate(new Vector3(0f, mazeInfo.StartingDirection.GetRotation(), 0f));
		player.name = "MazePlayerController";

		player.GetComponent<CameraPicker>().Initialize(UseOVR);
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

	private static string SelectTrapController(TrapType trapType)
	{		
		switch (trapType)
		{
		case TrapType.Spikes:
			return typeof(SpikesTrapController).Name;
		case TrapType.FlameThrower:
			return typeof(FlameThrowerTrapController).Name;
//		case TrapType.LiquidThrower:
//			return typeof(LiquidThrowerTrapController).Name;
//		case TrapType.Projectiles:
//			return typeof(ProjectilesTrapController).Name;
		}
		Debug.LogWarning("Unknown trap type " + trapType + ", defaulting to Spikes");
		return "SpikeTrapController";
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
				errors.Add ("The placement of Trap with ID " + trap.ID + " is invalid because Room with ID " + trap.RoomID + " already contains a Trap in its " + trap.Placement.ToString() + " section.");
			}
			else
			{
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

	private void Clear() {
		foreach (var room in Rooms.Values) {
			GameObject.Destroy(room.gameObject);
		}
		Rooms.Clear ();
		TrapManager.Instance.Clear();
	}
}