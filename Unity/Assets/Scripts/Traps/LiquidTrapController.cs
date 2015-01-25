using UnityEngine;
using System.Collections;

public class LiquidThrowerTrapController : TrapController {
	public GameObject LiquidThrowerPrefab;
	
	protected override GameObject TrapPrefab {
		get {
			return LiquidThrowerPrefab;
		}
	}
}
