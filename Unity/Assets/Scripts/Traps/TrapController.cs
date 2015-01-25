using UnityEngine;
using System.Collections;

public abstract class TrapController : MonoBehaviour {
	public Placement Placement;
	public Direction Direction;

	protected abstract GameObject TrapPrefab { get; }

	protected float _cooldown = 0.0f;
	
	protected virtual void Start()
	{
		if (TrapPrefab != null)
		{
			CreateTrapGroup();
		}
	}

	private void CreateTrapGroup()
	{
		var groupTransform = transform;
		groupTransform.localPosition = GetPlacementPosition();
		var trapTileCount = (int)(RoomConstants.RoomSize / 2);
		var childLocalOffset = trapTileCount / 2f; // trap tiles are 1 meter squares
		for (int i = 0; i < trapTileCount; i++)
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

	protected virtual void Update()
	{
		if (_cooldown > 0.0f)
		{
			_cooldown -= Time.deltaTime;
		}
	}
	
	public bool TryActivate()
	{
		if (_cooldown > 0.0f)
		{
			return false;
		}

		// Start animating, keep track of timing
		// Subclasses may do special stuff here
		return InternalTryActivate();
	}

	protected abstract bool InternalTryActivate();
	
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
