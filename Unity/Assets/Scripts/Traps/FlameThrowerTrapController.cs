using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlameThrowerTrapController : TrapController {
	private const float FLAME_TRIGGER_TIME = 0.3f;
	private const float FLAME_WAIT_TIME = 3.0f;
	private const float FLAME_COOLDOWN = FLAME_TRIGGER_TIME + FLAME_WAIT_TIME + 3.0f;

	private List<FlameThrowerController> _controllers = new List<FlameThrowerController>();
	private float _triggerTime = 0.0f;
	private float _waitTime = 0.0f;
	
	protected override GameObject TrapPrefab 
	{
		get 
		{
			return TrapManager.Instance.FlameThrowerTrapPrefab;
		}
	}

	protected override void Start()
	{
		base.Start();
		
		for(int i = 0; i < transform.childCount; ++i)
		{
			_controllers.Add(transform.GetChild(i).GetComponent<FlameThrowerController>());
		}
	}
	
	protected override void Update()
	{
		base.Update();
		
		if (_triggerTime > 0.0f)
		{
			_triggerTime -= Time.deltaTime;
			
			if (_triggerTime <= 0.0f)
			{
				_waitTime = FLAME_WAIT_TIME;
				SetControllerStates(FlameThrowerController.State.Waiting);
			}
		}
		else if (_waitTime > 0.0f)
		{
			_waitTime -= Time.deltaTime;
			
			if (_waitTime <= 0.0f)
			{
				SetControllerStates(FlameThrowerController.State.Resetting);
			}
		}
	}
	
	protected override bool InternalTryActivate()
	{
		_cooldown = FLAME_COOLDOWN;
		
		_triggerTime = FLAME_TRIGGER_TIME;
		SetControllerStates(FlameThrowerController.State.Triggering);
		
		return true;
	}
	
	private void SetControllerStates(FlameThrowerController.State state)
	{
		foreach (var controller in _controllers)
		{
			controller.SetState(state);
		}
	}
}

