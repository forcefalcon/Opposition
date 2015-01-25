using UnityEngine;
using System.Collections;

public class ApplicationManager : MonoBehaviour
{	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}
