using UnityEngine;
using System.Collections;

public class CameraYLook : MonoBehaviour
{
	public float MouseSensitivity = 15F;
	public float GamepadSensitivity = 1.5F;
	public float MinimumY = -60F;
	public float MaximumY = 60F;

	float rotationY = 0F;
	
	void Update()
	{
		rotationY += Input.GetAxis("Mouse Y") * MouseSensitivity;
		rotationY += Input.GetAxis("GamepadRightY") * GamepadSensitivity;
		rotationY = Mathf.Clamp(rotationY, MinimumY, MaximumY);

		transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
	}
}
