using UnityEngine;
using System.Collections;

public abstract class TrapController : MonoBehaviour {
	public Placement Placement;
	public Direction Direction;
	
	protected abstract GameObject TrapPrefab { get; }
		
	// Use this for initialization
	void Start () {
		if (TrapPrefab != null)
		{
			CreateTrapGroup();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void CreateTrapGroup()
	{
		var groupTransform = transform;
		groupTransform.localPosition = GetPlacementPosition();
		var trapTileCount = (int)(RoomConstants.RoomSize / 2);
		var childLocalOffset = trapTileCount / 2f; // trap tiles are 1 meter squares
		for(int i = 0; i < trapTileCount; i++)
		{
			for (int j = 0; j < trapTileCount; j++)
			{
				var trapGO = (GameObject)GameObject.Instantiate(TrapPrefab);
				var childTransform = trapGO.transform;
				childTransform.parent = groupTransform;
				childTransform.localPosition = new Vector3(i - childLocalOffset, 0, j - childLocalOffset);
			}
		}
	}
	
	public virtual bool TryActivate()
	{
		// Start animating, keep track of timing
		// Subclasses may do special stuff here
		return false;
	}
	
	private Vector3 GetPlacementPosition()
	{
		float distanceOffset = RoomConstants.RoomSize / 4;
		switch (Placement)
		{
		case Placement.NorthWest:
			return new Vector3(-distanceOffset, 0, distanceOffset);
		case Placement.NorthEast:
			return new Vector3(distanceOffset, 0, distanceOffset);
		case Placement.SouthWest:
			return new Vector3(-distanceOffset, 0, -distanceOffset);
		case Placement.SouthEast:
			return new Vector3(distanceOffset, 0, -distanceOffset);
		}
		Debug.LogError ("Unknown Placement " + Placement + ", defaulting to NorthWest");
		return new Vector3(-distanceOffset, 0, distanceOffset);
	}
}
