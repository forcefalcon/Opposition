using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour
{
	public string ProjectileTriggerSound;
	public string ProjectileImpactSound;

	private bool _shooting = false;

	public void SetShooting(bool isShooting)
	{
		if (isShooting)
		{
			animation.Play();
			AkSoundEngine.PostEvent(ProjectileTriggerSound, this.gameObject);
		}

		_shooting = isShooting;
	}

	void OnTriggerStay(Collider other)
	{
		if (_shooting && other.tag == "Player" && ApplicationManager.Instance.IsPlaying)
		{
			AkSoundEngine.PostEvent(ProjectileImpactSound, other.gameObject);
			ApplicationManager.Instance.EndGame(true);
		}
	}
}
