using UnityEngine;
using UnityEditor;
using System.Collections;

public class RoomGenerator : EditorWindow {
	private string _roomDefinitionFile;

	void OnGUI () {
		GUILayout.Label ("Room definition tool", EditorStyles.boldLabel);
		_roomDefinitionFile = EditorGUILayout.TextField ("File path", _roomDefinitionFile);
	}

	[MenuItem("Rooms/Generator")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow (typeof(RoomGenerator));
		Debug.Log ("GenerateRoom called");
	}
}
