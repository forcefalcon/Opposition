﻿using UnityEngine;
using System.Collections;

public class ApplicationManager : MonoBehaviour
{
	public static ApplicationManager Instance { get; private set; }

	public bool IsPlaying { get; private set; }

	public Texture2D DeathTexture;
	public Texture2D SuccessTexture;

	private ScreenOverlay[] _overlays;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		IsPlaying = true;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		else if (Input.GetKeyDown (KeyCode.Backspace))
		{
			StartCoroutine(RestartIn(1f));
		}
		else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetKeyDown(KeyCode.F1))
			{
				MazeSelectionParameters.NextMazeIndex = 0;
				StartCoroutine(RestartIn(1f));
			}
			else if (Input.GetKeyDown(KeyCode.F2))
			{
				MazeSelectionParameters.NextMazeIndex = 1;
				StartCoroutine(RestartIn(1f));
			}
		}
	}

	public void EndGame(bool playerDied)
	{
		IsPlaying = false;

		GameObject player = GameObject.FindGameObjectWithTag("Player");

		// Freeze Player Controls
		CharacterMovementController move = player.GetComponent<CharacterMovementController>();
		move.FreezeMovement = true;

		float waitTime = 0.0f;

		_overlays = player.GetComponentsInChildren<ScreenOverlay>();
		if (playerDied)
		{
			// Play Failure
			foreach (var overlay in _overlays)
			{
				overlay.texture = DeathTexture;
			}

			waitTime = 3.0f;
		}
		else
		{
			// Play Success
			foreach (var overlay in _overlays)
			{
				overlay.texture = SuccessTexture;
			}

			waitTime = 6.0f;
		}

		StartCoroutine(AnimateOverlay(waitTime/2));

		// Reload Level
		StartCoroutine(RestartIn(waitTime));
	}

	IEnumerator AnimateOverlay(float OverlayTime)
	{
		float intensity = 0.0f;
		while (intensity < 1.0f)
		{
			intensity += Time.deltaTime / OverlayTime;

			foreach (var overlay in _overlays)
			{
				overlay.intensity = intensity;
			}

			yield return null;
		}
	}

	IEnumerator RestartIn(float time)
	{
		yield return new WaitForSeconds(time);
		Application.LoadLevel(0);
	}
}
