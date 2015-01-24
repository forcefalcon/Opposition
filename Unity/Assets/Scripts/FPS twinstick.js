#pragma strict

	var speed : float = 3.0;
	var rotateSpeed : float = 3.0;
	function Update () {
		var controller : CharacterController = GetComponent(CharacterController);
		// Strafe left right
		var side : Vector3 = transform.TransformDirection(Vector3.right);
		var curSide : float = speed * Input.GetAxis ("Horizontal");
		controller.SimpleMove(side * curSide);
		
		// Move forward / backward
		var forward : Vector3 = transform.TransformDirection(Vector3.forward);
		var curSpeed : float = speed * Input.GetAxis ("Vertical");
		controller.SimpleMove(forward * curSpeed);
		
		//rotation
		// Rotate around y - axis
		transform.Rotate(0, Input.GetAxis ("Rotate") * rotateSpeed, 0);
	}
	@script RequireComponent(CharacterController)