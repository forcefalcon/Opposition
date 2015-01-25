using UnityEngine;
using System.Collections;

public class FlameThrowerTrapController : TrapController {
	public GameObject FlameThrowerPrefab;
	
	protected override GameObject TrapPrefab {
		get {
			return FlameThrowerPrefab;
		}
	}
}
