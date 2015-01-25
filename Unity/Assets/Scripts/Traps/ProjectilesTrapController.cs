using UnityEngine;
using System.Collections;

public class ProjectilesThrowerTrapController : TrapController {
	public GameObject ProjectilesThrowerPrefab;
	
	protected override GameObject TrapPrefab {
		get {
			return ProjectilesThrowerPrefab;
		}
	}
}
