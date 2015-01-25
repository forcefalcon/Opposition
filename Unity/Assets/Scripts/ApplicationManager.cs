using UnityEngine;
using System.Collections;

public class ApplicationManager : MonoBehaviour
{
	public static ApplicationManager Instance { get; private set; }

	public bool IsPlaying { get; private set; }

	public Texture2D DeathTexture;
	public Texture2D SuccessTexture;

	public float OverlayRate = 2.0f;

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
	}

	public void EndGame(bool playerDied)
	{
		IsPlaying = false;

		GameObject player = GameObject.FindGameObjectWithTag("Player");

		// Freeze Player Controls
		CharacterMovementController move = player.GetComponent<CharacterMovementController>();
		move.FreezeMovement = true;

		_overlays = player.GetComponentsInChildren<ScreenOverlay>();
		if (playerDied)
		{
			// Play Failure
			foreach (var overlay in _overlays)
			{
				overlay.texture = DeathTexture;
			}
		}
		else
		{
			// Play Success
			foreach (var overlay in _overlays)
			{
				overlay.texture = SuccessTexture;
			}
		}

		StartCoroutine(AnimateOverlay());

		// Reload Level
		StartCoroutine(RestartIn(3.0f));
	}

	IEnumerator AnimateOverlay()
	{
		float intensity = 0.0f;
		while (intensity < 1.0f)
		{
			intensity += Time.deltaTime * OverlayRate;

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
