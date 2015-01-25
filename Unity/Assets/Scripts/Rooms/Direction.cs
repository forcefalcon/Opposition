
public enum Direction
{
	North,
	South,
	East,
	West,
}

public static class DirectionExcentions
{
	public static float GetRotation(this Direction direction)
	{
		switch (direction)
		{
		default:
		case Direction.North:
			return 0f;
		case Direction.East:
			return 90f;
		case Direction.South:
			return 180f;
		case Direction.West:
			return 270f;
		}
	}
}