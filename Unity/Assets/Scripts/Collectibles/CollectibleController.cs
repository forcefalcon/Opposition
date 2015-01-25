using UnityEngine;
using System.Collections;

public class CollectibleController : MonoBehaviour
{
	public Placement Placement;
    public int CollectibleID;

	public void Start()
	{
		transform.localPosition = Placement.ToPosition();
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

