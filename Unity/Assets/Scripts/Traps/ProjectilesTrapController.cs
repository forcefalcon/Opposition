using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectilesTrapController : TrapController
{
	private const float PROJECTILE_TRIGGER_TIME = 0.1f;
	private const float PROJECTILE_COOLDOWN = PROJECTILE_TRIGGER_TIME + 2f;

	private List<ProjectileController> _controllers = new List<ProjectileController>();
	private float _triggerTime = 0.0f;

	protected override GameObject TrapPrefab 
	{
		get 
		{
			return TrapManager.Instance.ProjectilesTrapPrefab;
		}
	}

	protected override void Start()
	{
		base.Start();

		for (int i = 0; i < transform.childCount; ++i)
		{
			_controllers.Add(transform.GetChild(i).GetComponent<ProjectileController>());
		}
	}

	protected override bool InternalTryActivate()
	{
		_cooldown = PROJECTILE_COOLDOWN;

		_triggerTime = PROJECTILE_TRIGGER_TIME;

		foreach (var controller in _controllers)
		{
			controller.SetShooting(true);
		}

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
				foreach (var controller in _controllers)
				{
					controller.SetShooting(false);
				}
			}
		}
	}

}
