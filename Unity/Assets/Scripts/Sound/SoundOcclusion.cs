using UnityEngine;
using System.Collections;

public class SoundOcclusion : MonoBehaviour
{
	public float UpdateRate = 0.333f;
	public Vector3 RaycastOffset = Vector3.zero;

	private float _timer = 0.0f;
	private Transform _transform;
	private Transform _playerTransform = null;

	void Start()
	{
		_transform = transform;

		_timer = Random.Range(0.0f, UpdateRate);
	}
	
	void Update()
	{
		_timer += Time.deltaTime;

		if (_timer >= UpdateRate)
		{
			_timer = 0.0f;

			UpdateOcclusion();
		}
	}

	void UpdateOcclusion()
	{
		if (_playerTransform == null)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");

			if (player != null)
			{
				_playerTransform = player.transform;
			}
			else
			{
				return;
			}
		}

		Vector3 start = _transform.position + RaycastOffset;
		Vector3 end = _playerTransform.position + Vector3.up;
		Vector3 toEnd = end - start;
		float toEndDist = toEnd.magnitude;
		Vector3 toEndNorm = toEnd / toEndDist;

		float obstruction = 0.0f;
		float occlusion = 0.0f;

		int layerMask = 1 << LayerMask.NameToLayer("Walls");
		if (Physics.Raycast(start, toEndNorm, toEndDist, layerMask))
		{
			obstruction = 100.0f;
		}

		AkSoundEngine.SetObjectObstructionAndOcclusion(this.gameObject, 0, obstruction, occlusion);
	}
}
