using UnityEngine;
using System.Collections;

public class FlameThrowerTrapController : TrapController {
	
	protected override GameObject TrapPrefab {
		get {
			return TrapManager.Instance.FlameThrowerTrapPrefab;
		}
	}

	protected override bool InternalTryActivate()
	{
		throw new System.NotImplementedException();
	}
}
