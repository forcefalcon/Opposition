using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
	private bool _shooting = false;

	public void SetShooting(bool isShooting)
	{
		if (isShooting)
		{
			animation.Play();
		}

		_shooting = isShooting;
	}

	void OnTriggerStay(Collider other)
	{
		if (_shooting && other.tag == "Player" && ApplicationManager.Instance.IsPlaying)
		{
			ApplicationManager.Instance.EndGame(true);
		}
	}
}
