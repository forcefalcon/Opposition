using UnityEngine;
using System.Collections;

public class CameraPicker : MonoBehaviour
{
	public GameObject NormalCamera;
	public GameObject OVRCamera;

	public void Initialize(bool useOVR)
	{
		if (useOVR)
		{
			OVRCamera.SetActive(true);
		}
		else
		{
			NormalCamera.SetActive(true);
		}
	}
}
