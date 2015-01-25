using UnityEngine;
using System.Collections;

public class CharacterAnimationController : MonoBehaviour
{
	public float RunSpeed = 10.0f;

	private CharacterController _controller = null;
	private Animator _animator = null;

	void Start()
	{
		_controller = GetComponent<CharacterController>();
		_animator = GetComponentInChildren<Animator>();
	}

	void Update()
	{
		if (_controller != null && _animator != null)
		{
			float animSpeed = Mathf.Clamp01(_controller.velocity.magnitude / RunSpeed) * 2.0f;

			//Debug.Log(animSpeed);
			_animator.SetFloat("Blend", animSpeed);
		}
	}
}
