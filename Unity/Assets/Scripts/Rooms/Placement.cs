using UnityEngine;

public enum Placement{
	NorthWest,
	NorthEast,
	SouthWest,
	SouthEast,
}

public static class PlacementExtensions
{
	public static Vector3 ToPosition(this Placement placement)
	{
		float distanceOffset = RoomConstants.RoomSize / 4;
		switch (placement)
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
		Debug.LogError ("Unknown Placement " + placement + ", defaulting to NorthWest");
		return new Vector3(-distanceOffset, 0, distanceOffset);
	}
}