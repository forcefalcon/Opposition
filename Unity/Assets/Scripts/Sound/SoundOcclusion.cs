using UnityEngine;
using System.Collections;

public class SoundOcclusion : MonoBehaviour
{
	public float UpdateRate = 0.333f;

	private float _timer = 0.0f;
	private Transform _transform;

	void Start()
	{
		_transform = transform;
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
		Vector3 start = _transform.position;
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Vector3 end = player.transform.position;
		Vector3 toEnd = end - start;
		float toEndDist = toEnd.magnitude;
		Vector3 toEndNorm = toEnd / toEndDist;

		float obstruction = 0.0f;
		if (Physics.Raycast(start, toEndNorm, toEndDist))
		{
			obstruction = 100.0f;
		}

		AkSoundEngine.SetObjectObstructionAndOcclusion(this.gameObject, 0, obstruction, 0.0f);
	}
}
