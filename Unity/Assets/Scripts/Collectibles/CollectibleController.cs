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
	
	public void Pickup()
	{
		// Start animating, playing sound...
	}
}

