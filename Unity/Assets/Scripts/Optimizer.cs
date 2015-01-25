using UnityEngine;
using System.Collections;

public class Optimizer : MonoBehaviour
{
	public float UpdateRate = 0.333f;
	public float FarDistance = 60.0f;

	public GameObject[] Objects;

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

			Optimize();
		}
	}

	private void Optimize()
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

		bool isFar = (_transform.position - _playerTransform.position).magnitude >= FarDistance;

		if (isFar)
		{
			for (int i = 0; i < Objects.Length; ++i)
			{
				if (Objects[i] != null)
				{
					Objects[i].SetActive(false);
				}
			}
		}
		else
		{
			for (int i = 0; i < Objects.Length; ++i)
			{
				if (Objects[i] != null)
				{
					Objects[i].SetActive(true);
				}
			}
		}
	}
}
