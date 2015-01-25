using UnityEngine;
using System.Collections;
using System.Linq;

public class LiquidThrowerController : MonoBehaviour
{
	public GameObject VFX_LiquidThrowerInstance;

	public string LiquidTriggerSound;
	public string LiquidImpactSound;

	public enum State
	{
		Idle,
		Triggering,
		Waiting,
		Resetting,
	}
	
	void Update()
	{
		if (_currentState == State.Resetting)
		{
			var particleSystems = VFX_LiquidThrowerInstance.GetComponentsInChildren<ParticleSystem>();
			if (particleSystems.All(ps => !ps.IsAlive()))
			{
				SetState(State.Idle);
			}
		}
	}
	
	private State _currentState = State.Idle;
	
	public void SetState(State newState)
	{
		switch(newState)
		{
		case State.Idle:
			VFX_LiquidThrowerInstance.SetActive(false);
			break;
		case State.Triggering:
			VFX_LiquidThrowerInstance.SetActive(true);
			AkSoundEngine.PostEvent(LiquidTriggerSound, this.gameObject);
			break;
		}
		
		_currentState = newState;
	}
	
	void OnTriggerStay(Collider other)
	{
		if (_currentState == State.Waiting && other.tag == "Player" && ApplicationManager.Instance.IsPlaying)
		{
			AkSoundEngine.PostEvent(LiquidImpactSound, other.gameObject);
			ApplicationManager.Instance.EndGame(true);
		}
	}
}
