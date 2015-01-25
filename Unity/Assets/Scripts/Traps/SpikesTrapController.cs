using UnityEngine;
using System.Collections;

public class SpikesTrapController : TrapController {
	public GameObject SpikesTrapPrefab;
	
	protected override GameObject TrapPrefab {
		get {
			return TrapManager.Instance.SpikesTrapPrefab;
		}
	}
}
