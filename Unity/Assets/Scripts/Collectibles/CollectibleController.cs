using UnityEngine;
using System.Collections;

public class CollectibleController : MonoBehaviour
{
	public Placement Placement;
    public int CollectibleID;

	public void Start()
	{
		transform.localPosition = Placement.ToPosition() + new Vector3(0, 1, 0);
	}
	
	public void Pickup(float completionRatio)
	{
		// Start animating, playing sound...
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (CollectibleManager.Instance.Collect(this))
		{
			enabled = false;
		}
	}
}

