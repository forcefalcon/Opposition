﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class FlameThrowerController : MonoBehaviour
{
	public GameObject VFX_FlameThrowerPrefab;
	private GameObject _VFX_FlameThrowerInstance;
	
	public string FlameThrowerTriggerSound;
	public string FlameThrowerImpactSound;
	
	public void Start()
	{
		_VFX_FlameThrowerInstance = (GameObject)GameObject.Instantiate(
			VFX_FlameThrowerPrefab,
			transform.position,
			Quaternion.identity);
		_VFX_FlameThrowerInstance.transform.parent = transform;
	}

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
			var particleSystems = _VFX_FlameThrowerInstance.GetComponentsInChildren<ParticleSystem>();
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
			_VFX_FlameThrowerInstance.SetActive(false);
			break;
		case State.Triggering:
			_VFX_FlameThrowerInstance.SetActive(true);
			AkSoundEngine.PostEvent(FlameThrowerTriggerSound, this.gameObject);
			break;
		}
		
		_currentState = newState;
	}
	
	void OnTriggerStay(Collider other)
	{
		if (_currentState == State.Waiting && other.tag == "Player" && ApplicationManager.Instance.IsPlaying)
		{
			AkSoundEngine.PostEvent(FlameThrowerImpactSound, other.gameObject);
			ApplicationManager.Instance.EndGame(true);
		}
	}
}
