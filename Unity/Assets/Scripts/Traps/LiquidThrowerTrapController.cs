using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiquidThrowerTrapController : TrapController {
	private const float LIQUID_TRIGGER_TIME = 0.3f;
	private const float LIQUID_WAIT_TIME = 3.0f;
	private const float LIQUID_COOLDOWN = LIQUID_TRIGGER_TIME + LIQUID_WAIT_TIME + 1.0f;
	
	private List<LiquidThrowerController> _controllers = new List<LiquidThrowerController>();
	private float _triggerTime = 0.0f;
	private float _waitTime = 0.0f;
	
	protected override GameObject TrapPrefab 
	{
		get 
		{
			return TrapManager.Instance.LiquidThrowerTrapPrefab;
		}
	}
		
	protected override void Start()
	{
		base.Start();
		
		for(int i = 0; i < transform.childCount; ++i)
		{
			_controllers.Add(transform.GetChild(i).GetComponent<LiquidThrowerController>());
		}
	}
	
	protected override void PositionTrapGroup(Transform groupTransform)
	{
		base.PositionTrapGroup(groupTransform);
		groupTransform.Translate(new Vector3(0, RoomConstants.CeilingHeight, 0));
	}
	
	protected override void Update()
	{
		base.Update();
		
		if (_triggerTime > 0.0f)
		{
			_triggerTime -= Time.deltaTime;
			
			if (_triggerTime <= 0.0f)
			{
				_waitTime = LIQUID_WAIT_TIME;
				SetControllerStates(LiquidThrowerController.State.Waiting);
			}
		}
		else if (_waitTime > 0.0f)
		{
			_waitTime -= Time.deltaTime;
			
			if (_waitTime <= 0.0f)
			{
				SetControllerStates(LiquidThrowerController.State.Resetting);
			}
		}
	}
	
	protected override bool InternalTryActivate()
	{
		_cooldown = LIQUID_COOLDOWN;
		
		_triggerTime = LIQUID_TRIGGER_TIME;
		SetControllerStates(LiquidThrowerController.State.Triggering);
		
		return true;
	}
	
	private void SetControllerStates(LiquidThrowerController.State state)
	{
		foreach (var controller in _controllers)
		{
			controller.SetState(state);
		}
	}
}
