using UnityEngine;
using System.Collections;

public class ResetCamera : MonoBehaviour
{
	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Period) || 
			Input.GetKeyDown(KeyCode.JoystickButton0) ||
			Input.GetKeyDown(KeyCode.JoystickButton1) ||
			Input.GetKeyDown(KeyCode.JoystickButton2) ||
			Input.GetKeyDown(KeyCode.JoystickButton3))
		{
			OVRManager.display.RecenterPose();
		}
	}
}
