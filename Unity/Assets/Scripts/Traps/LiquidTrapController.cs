using UnityEngine;
using System.Collections;

public class LiquidThrowerTrapController : TrapController {
	public GameObject LiquidThrowerPrefab;
	
	protected override GameObject TrapPrefab {
		get {
			return LiquidThrowerPrefab;
		}
	}

	protected override bool InternalTryActivate()
	{
		throw new System.NotImplementedException();
	}
}
