using UnityEngine;
using System.Collections;
using System.Linq;

public class CollectibleController : MonoBehaviour
{
	public const float PickupAnimDuration = 5f;
	
	public GameObject Cube;
	public GameObject VFX_Souls;
	public Placement Placement;
    public int CollectibleID;

	public string OpenSound = "SFX_Feedbacks_Collectibles_OpenChest";

	private ParticleSystem [] CubeParticleSystems;
	private float _tweenDuration;
	private bool _tweening;
	private bool _ending;

	public void Start()
	{
		transform.localPosition = Placement.ToPosition() + new Vector3(0, 1, 0);
		CubeParticleSystems = Cube.GetComponentsInChildren<ParticleSystem>();
	}
	
	public void Pickup(float completionRatio)
	{
		// Start animating, playing sound...
		AkSoundEngine.SetRTPCValue("Accomplishment", completionRatio * 100.0f, this.gameObject);

		AkSoundEngine.PostEvent(OpenSound, this.gameObject);
		
		_tweenDuration = 0;
		VFX_Souls.SetActive(true);
		_tweening = true;
	}
	
	void Update()
	{
		if (_tweening)
		{
			_tweenDuration += Time.deltaTime;
			if (_tweenDuration > PickupAnimDuration)
			{
				_tweening = false;
				Cube.renderer.material.SetFloat("_Cutoff", 1);
			}
			else
			{
				if (_tweenDuration > PickupAnimDuration / 3 && !_ending)
				{
					foreach (var ps in CubeParticleSystems)
					{
						ps.emissionRate = 0;
					}
					_ending = true;
				}
				Cube.renderer.material.SetFloat("_Cutoff", _tweenDuration / PickupAnimDuration);
			}
		}
		else if (_ending)
		{
			if (CubeParticleSystems.All(ps => !ps.IsAlive()))
			{
				Object.Destroy(gameObject);
			}
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		CollectibleManager.Instance.Collect(this);
	}
}

