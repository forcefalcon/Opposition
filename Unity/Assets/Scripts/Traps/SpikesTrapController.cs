using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikesTrapController : TrapController
{
	private const float SPIKE_TRIGGER_TIME = 0.1f;
	private const float SPIKE_WAIT_TIME = 1.0f;
	private const float SPIKE_RESET_TIME = 0.5f;
	private const float SPIKE_COOLDOWN = SPIKE_TRIGGER_TIME + SPIKE_WAIT_TIME + SPIKE_RESET_TIME + 0.5f;

	private List<SpikeController> _controllers = new List<SpikeController>();
	private float _triggerTime = 0.0f;
	private float _waitTime = 0.0f;
	private float _resetTime = 0.0f;
	
	protected override GameObject TrapPrefab
	{
		get
		{
			return TrapManager.Instance.SpikesTrapPrefab;
		}
	}

	protected override void Start()
	{
		base.Start();

		for(int i = 0; i < transform.childCount; ++i)
		{
			_controllers.Add(transform.GetChild(i).GetComponent<SpikeController>());
		}
	}

	protected override bool InternalTryActivate()
	{
		_cooldown = SPIKE_COOLDOWN;

		_triggerTime = SPIKE_TRIGGER_TIME;
		SetControllerStates(SpikeController.State.Triggering);

		return true;
	}

	protected override void Update()
	{
		base.Update();

		if (_triggerTime > 0.0f)
		{
			_triggerTime -= Time.deltaTime;

			if (_triggerTime <= 0.0f)
			{
				_waitTime = SPIKE_WAIT_TIME;
				SetControllerStates(SpikeController.State.Waiting);
			}
		}
		else if (_waitTime > 0.0f)
		{
			_waitTime -= Time.deltaTime;

			if (_waitTime <= 0.0f)
			{
				_resetTime = SPIKE_RESET_TIME;
				SetControllerStates(SpikeController.State.Resetting);
			}
		}
		else if (_resetTime > 0.0f)
		{
			_resetTime -= Time.deltaTime;

			if (_resetTime < 0.0f)
			{
				SetControllerStates(SpikeController.State.Idle);
			}
		}
	}

	private void SetControllerStates(SpikeController.State state)
	{
		foreach (var controller in _controllers)
		{
			controller.SetState(state);
		}
	}
}
