using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class RoomManager : MonoBehaviour
{
	public static RoomManager Instance { get; private set; }

	public GameObject MetalRoomPrefab;

	public int StartingRoomID;

	private Dictionary<int, Room> Rooms { get; set; }

	public void Awake() {
		Instance = this;
		Rooms = new Dictionary<int, Room> ();
	}
		
	public void Start(){

		// testing
		Serialization.MazeInfo mazeInfo = null;
		using (var reader = new StreamReader(@"Assets\RoomSystem\Data\Sample.maze")) {
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
		StartingRoomID = mazeInfo.StartingRoomID;

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