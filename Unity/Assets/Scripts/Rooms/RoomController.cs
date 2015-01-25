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

			// TODO Add corridors in open directions ?
		}
	}

	private void CreateDoor(Direction direction)
	{
		var wallTransform = gameObject.transform.Find("Doors/" + direction.ToString () + "Door");
		if (wallTransform != null)
		{
			wallTransform.gameObject.SetActive(false);
		}
	}	
}