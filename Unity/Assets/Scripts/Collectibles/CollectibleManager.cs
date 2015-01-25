using UnityEngine;
using System.Collections.Generic;

public class CollectibleManager : MonoBehaviour
{
	public static CollectibleManager Instance { get; private set; }
	
	private HashSet<int> _CollectiblesFound = new HashSet<int>();
	
	public void Awake()
	{
		Instance = this;
	}
	
	public void Collect(CollectibleController collectible)
	{
		if (_CollectiblesFound.Contains(collectible.CollectibleID))
		{
			Debug.LogWarning("Collectible with ID " + collectible.CollectibleID + " has already been picked up.");
			return;
		}
		_CollectiblesFound.Add(collectible.CollectibleID);
		collectible.Pickup();
		
		// Detect if all collectibles were found and send game end event if so
	}
	
	public void Clear()
	{
		_CollectiblesFound.Clear();
	}
}

