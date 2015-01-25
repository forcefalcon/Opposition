using UnityEngine;
using System.Collections;

public class CharacterSoundController : MonoBehaviour
{
	public string FootstepEvent = "";
	public float FootstepRate = 2.0f;
	public string SpeedRTPC = "";
	public float LowSpeedRTPC = 2.0f;
	public float HighSpeedRTPC = 8.0f;
	public string FloorTypeSwitch = "";

	private CharacterController _controller = null;
	private float _timeSinceFootstep = 0.0f;

	void Start()
	{
		_controller = GetComponent<CharacterController>();
	}
	
	void Update() 
	{
		if (_controller != null)
		{
			float speed = _controller.velocity.magnitude;
			float speedNorm = Mathf.InverseLerp(LowSpeedRTPC, HighSpeedRTPC, speed) * 100.0f;
			AkSoundEngine.SetRTPCValue(SpeedRTPC, speedNorm, this.gameObject);

			if (FootstepEvent != null)
			{
				RaycastHit hit;
				if (Physics.Raycast(transform.position + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out hit, 1.5f))
				{
					string tag = hit.collider.gameObject.tag;
					if (tag != "Untagged")
					{
						AkSoundEngine.SetSwitch(FloorTypeSwitch, tag, this.gameObject);
					}
				}

				_timeSinceFootstep += Time.deltaTime * speed;

				if (_timeSinceFootstep > FootstepRate)
				{
					AkSoundEngine.PostEvent(FootstepEvent, this.gameObject);

					_timeSinceFootstep -= FootstepRate;
				}
			}
		}
	}
}
