using UnityEngine;
using System.Collections;

public class ProjectilesTrapController : TrapController {
	public GameObject ProjectilesPrefab;
	
	protected override GameObject TrapPrefab {
		get {
			return ProjectilesPrefab;
		}
	}
}
