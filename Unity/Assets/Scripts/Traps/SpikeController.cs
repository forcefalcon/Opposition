using UnityEngine;
using System.Collections;

public class SpikeController : MonoBehaviour
{
	public string SpikeTriggerSound;
	public string SpikeImpactSound;

	private const string SPIKE_TRIGGER_ANIM = "SpikeTrigger";
	private const string SPIKE_RESET_ANIM = "SpikeReset";

	public enum State
	{
		Idle,
		Triggering,
		Waiting,
		Resetting
	}

	private State _currentState = State.Idle;

	public void SetState(State newState)
	{
		switch(newState)
		{
			case State.Idle:
				collider.isTrigger = true;
				break;
			case State.Triggering:
				collider.isTrigger = true;
				animation.Play(SPIKE_TRIGGER_ANIM);
				AkSoundEngine.PostEvent(SpikeTriggerSound, this.gameObject);
				break;
			case State.Waiting:
				collider.isTrigger = false;
				break;
			case State.Resetting:
				collider.isTrigger = false;
				animation.Play(SPIKE_RESET_ANIM);
				break;
		}

		_currentState = newState;
	}

	void OnTriggerStay(Collider other)
	{
		if (_currentState == State.Triggering && other.tag == "Player" && ApplicationManager.Instance.IsPlaying)
		{
			AkSoundEngine.PostEvent(SpikeImpactSound, other.gameObject);
			ApplicationManager.Instance.EndGame(true);
		}
	}
}
