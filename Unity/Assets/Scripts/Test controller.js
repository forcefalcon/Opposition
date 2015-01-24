#pragma strict
	/// This script moves the character controller forward 
	/// and sideways based on the arrow keys.
	/// It also jumps when pressing space.
	/// Make sure to attach a character controller to the same game object.
	/// It is recommended that you make only one call to Move or SimpleMove per frame.	
	var speed : float = 6.0;
	var jumpSpeed : float = 8.0;
	var gravity : float = 20.0;
	private var moveDirection : Vector3 = Vector3.zero;
	private var startPosition : Vector3;
	var xOffset : float;
	var yOffset : float;
	
	function Start ()
	{
		startPosition = this.transform.position;
	}
	
	function Update() 
	{
		//updater le xOffset et le yOffset
		xOffset = Input.GetAxis("Horizontal");
		yOffset = Input.GetAxis("Vertical");
				{
					this.gameObject.transform.position.x = startPosition.x + Input.GetAxis("Horizontal");
					this.gameObject.transform.position.y = startPosition.y + Input.GetAxis("Vertical");
				}

	}