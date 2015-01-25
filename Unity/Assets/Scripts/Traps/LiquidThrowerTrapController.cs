using UnityEngine;
using System.Collections;

public class LiquidThrowerTrapController : TrapController {
	
	protected override GameObject TrapPrefab {
		get {
			return TrapManager.Instance.LiquidThrowerTrapPrefab;
		}
	}
}
