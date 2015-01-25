using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class RoomController : MonoBehaviour
{
	public List<Direction> Connections;
	
	public void Start()
	{
		SetupConnections();
	}

	private void SetupConnections()
	{
		if (Connections != null) {
			foreach (var direction in Connections)
			{
				CreateDoor(direction);
			}
		}
	}

	private void CreateDoor(Direction direction)
	{
		SetChildActive(direction.ToString () + "Door", false);
		SetChildActive(direction.ToString () + "DoorFrame", true);
	}
	
	private void SetChildActive(string childName, bool active)
	{
		var childTransform = gameObject.transform.Find(childName);
		if (childTransform != null)
		{
			childTransform.gameObject.SetActive(active);
		}
		else
		{
			Debug.LogError("Couldn't find child transform named " + childName);
		}
	}
}