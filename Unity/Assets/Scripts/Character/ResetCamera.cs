using UnityEngine;
using System.Collections;

public class ResetCamera : MonoBehaviour
{
	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.JoystickButton0))
		{
			OVRManager.display.RecenterPose();
		}
	}
}
