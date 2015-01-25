using UnityEngine;
using System.Collections.Generic;

public class TrapManager : MonoBehaviour
{
	public static TrapManager Instance { get; private set; }
	
	public float GlobalTrapCooldown = 5f;

	public GameObject SpikesTrapPrefab;
	public GameObject FlameThrowerTrapPrefab;
	public GameObject LiquidThrowerTrapPrefab;
	public GameObject ProjectilesTrapPrefab;
	
	private float _Cooldown;
	private Dictionary<KeyCode, TrapController> _TrapBindings = new Dictionary<KeyCode, TrapController>();
	
	public void Awake()
	{
		Instance = this;
	}
	
	public void Update()
	{
		if (_Cooldown > 0f)
		{
			_Cooldown -= Time.deltaTime;
		}
		if (_Cooldown <= 0)
		{
			foreach (var kvp in _TrapBindings)
			{
				var trapController = kvp.Value;
				if (Input.GetKeyDown(kvp.Key) && trapController.TryActivate())
				{
					_Cooldown = GlobalTrapCooldown;
					break;
				}
			}
		}
		else
		{
			// Play "cooldown" sound?
		}
	}
	
	public void RegisterTrap(TrapController controller, KeyCode keyBinding)
	{
		_TrapBindings[keyBinding] = controller;
	}
	
	public void Clear()
	{
		_TrapBindings.Clear();
	}
}



