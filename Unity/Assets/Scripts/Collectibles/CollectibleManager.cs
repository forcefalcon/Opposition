using UnityEngine;
using System.Collections.Generic;

public class CollectibleManager : MonoBehaviour
{
	public static CollectibleManager Instance { get; private set; }
	
	public int CollectibleCount;
	
	private HashSet<int> _CollectiblesFound = new HashSet<int>();
	
	public void Awake()
	{
		Instance = this;
	}
	
	public bool Collect(CollectibleController collectible)
	{
		if (_CollectiblesFound.Contains(collectible.CollectibleID))
		{
			Debug.LogWarning("Collectible with ID " + collectible.CollectibleID + " has already been picked up.");
			return false;
		}
		_CollectiblesFound.Add(collectible.CollectibleID);
		collectible.Pickup(_CollectiblesFound.Count / (float)CollectibleCount);
		
		if (_CollectiblesFound.Count >= CollectibleCount)
		{
			// TODO Do something to end the game
		}
		return true;
	}
	
	public void Clear()
	{
		_CollectiblesFound.Clear();
	}
}

