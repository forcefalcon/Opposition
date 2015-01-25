using UnityEngine;
using System.Collections;

public class ProjectilesTrapController : TrapController {
	
	protected override GameObject TrapPrefab {
		get {
			return TrapManager.Instance.ProjectilesTrapPrefab;
		}
	}

	protected override bool InternalTryActivate()
	{
		throw new System.NotImplementedException();
	}
}
