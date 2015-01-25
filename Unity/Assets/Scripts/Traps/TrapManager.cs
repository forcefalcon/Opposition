using UnityEngine;
using System.Collections.Generic;

public class TrapManager : MonoBehaviour
{
	public static TrapManager Instance { get; private set; }
	
	public GameObject SpikesTrapPrefab;
	public GameObject FlameThrowerTrapPrefab;
	public GameObject LiquidThrowerTrapPrefab;
	public GameObject ProjectilesTrapPrefab;
	
	private Dictionary<KeyCode, TrapController> Traps;
	
	public TrapController ActiveTrap { get; private set; }
	
	public void Awake()
	{
		Instance = this;
		Traps = new Dictionary<KeyCode, TrapController>();
	}
	
	public void Update()
	{
		if (ActiveTrap == null)
		{
			foreach (var kvp in Traps)
			{
				if (Input.GetKeyDown(kvp.Key))
				{
					Debug.Log ("Activating " + kvp.Value.name + " trap!");
					//ActiveTrap = kvp.Value;
					break;
				}
			}
		}
	}
	
	public void RegisterTrap(TrapController controller, KeyCode keyBinding)
	{
		Traps[keyBinding] = controller;
	}
}



