using UnityEngine;
using System.Collections;

public abstract class TrapController : MonoBehaviour {
	public Placement Placement;
	public Direction Direction;

	protected abstract GameObject TrapPrefab { get; }

	protected float _cooldown = 0.0f;
	
	private const float RescaleFactor = 1.2f;
	
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
		var trapTileCount = (int)(RoomConstants.RoomSize / 2);
		// determine the negative offset to apply to each tile for the result to be centered on
		// the trap group;
		// trap tiles are 1 meter squares, so -0.5 because the position of the tiles is on their center
		var childLocalOffset = trapTileCount / 2f - 0.5f; 
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
		PositionTrapGroup(groupTransform);
	}
	
	protected virtual void PositionTrapGroup(Transform groupTransform)
	{
		groupTransform.localPosition = Placement.ToPosition() / RescaleFactor;
		groupTransform.localScale = new Vector3(RescaleFactor,RescaleFactor,RescaleFactor);
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
	
}
