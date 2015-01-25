using UnityEngine;
using System.Collections;

public class CharacterMovementController : MonoBehaviour
{
	/// <summary>
	/// The rate acceleration during movement.
	/// </summary>
	public float Acceleration = 0.1f;

	/// <summary>
	/// The rate of damping on movement.
	/// </summary>
	public float Damping = 0.3f;
	
	/// <summary>
	/// The rate of rotation when using a gamepad.
	/// </summary>
	public float RotationAmount = 1.5f;

	/// <summary>
	/// The multiplier applied to the mouse when rotating.
	/// </summary>
	public float MouseRotationMultiplier = 3.25f;

	/// <summary>
	/// The rate of additional damping when moving sideways or backwards.
	/// </summary>
	public float BackAndSideDampen = 0.5f;

	/// <summary>
	/// Modifies the strength of gravity.
	/// </summary>
	public float GravityModifier = 0.379f;

	private float MoveScale = 1.0f;
	private Vector3 MoveThrottle = Vector3.zero;
	private float FallSpeed = 0.0f;

	protected CharacterController Controller = null;

	private float SimulationRate = 60f;

	public bool FreezeMovement { get; set; }

	void Awake()
	{
		FreezeMovement = false;

		Controller = gameObject.GetComponent<CharacterController>();

		if (Controller == null)
			Debug.LogWarning("CharacterMovementController: No CharacterController attached.");
	}

	void Update() 
	{
		if (FreezeMovement)
			return;

		UpdateMovement();

		Vector3 moveDirection = Vector3.zero;

		float motorDamp = (1.0f + (Damping * SimulationRate * Time.deltaTime));

		MoveThrottle.x /= motorDamp;
		MoveThrottle.y = (MoveThrottle.y > 0.0f) ? (MoveThrottle.y / motorDamp) : MoveThrottle.y;
		MoveThrottle.z /= motorDamp;

		moveDirection += MoveThrottle * SimulationRate * Time.deltaTime;

		// Gravity
		if (Controller.isGrounded && FallSpeed <= 0)
			FallSpeed = ((Physics.gravity.y * (GravityModifier * 0.002f)));
		else
			FallSpeed += ((Physics.gravity.y * (GravityModifier * 0.002f)) * SimulationRate * Time.deltaTime);

		moveDirection.y += FallSpeed * SimulationRate * Time.deltaTime;

		// Offset correction for uneven ground
		float bumpUpOffset = 0.0f;

		if (Controller.isGrounded && MoveThrottle.y <= 0.001f)
		{
			bumpUpOffset = Mathf.Max(Controller.stepOffset, new Vector3(moveDirection.x, 0, moveDirection.z).magnitude);
			moveDirection -= bumpUpOffset * Vector3.up;
		}

		Vector3 predictedXZ = Vector3.Scale((Controller.transform.localPosition + moveDirection), new Vector3(1, 0, 1));

		// Move contoller
		Controller.Move(moveDirection);

		Vector3 actualXZ = Vector3.Scale(Controller.transform.localPosition, new Vector3(1, 0, 1));

		if (predictedXZ != actualXZ)
			MoveThrottle += (actualXZ - predictedXZ) / (SimulationRate * Time.deltaTime);

		UpdateRotation();
	}

	void UpdateMovement()
	{
		bool moveForward = Input.GetKey(KeyCode.UpArrow);
		bool moveLeft = Input.GetKey(KeyCode.LeftArrow);
		bool moveRight = Input.GetKey(KeyCode.RightArrow);
		bool moveBack = Input.GetKey(KeyCode.DownArrow);

		MoveScale = 1.0f;

		if ((moveForward && moveLeft) || (moveForward && moveRight) ||
			 (moveBack && moveLeft) || (moveBack && moveRight))
			MoveScale = 0.70710678f;

		// No positional movement if we are in the air
		if (!Controller.isGrounded)
			MoveScale = 0.0f;

		MoveScale *= SimulationRate * Time.deltaTime;

		// Compute this for key movement
		float moveInfluence = Acceleration * 0.1f * MoveScale;

		// Run!
		if (Input.GetKey(KeyCode.RightShift) || Mathf.Abs(Input.GetAxisRaw("GamepadTriggers")) > 0.3f)
			moveInfluence *= 2.0f;

		Quaternion ort = transform.rotation;
		Vector3 ortEuler = ort.eulerAngles;
		ortEuler.z = ortEuler.x = 0f;
		ort = Quaternion.Euler(ortEuler);

		if (moveForward)
			MoveThrottle += ort * (transform.lossyScale.z * moveInfluence * Vector3.forward);
		if (moveBack)
			MoveThrottle += ort * (transform.lossyScale.z * moveInfluence * BackAndSideDampen * Vector3.back);
		if (moveLeft)
			MoveThrottle += ort * (transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.left);
		if (moveRight)
			MoveThrottle += ort * (transform.lossyScale.x * moveInfluence * BackAndSideDampen * Vector3.right);

		float leftAxisX = Input.GetAxis("GamepadLeftX");
		float leftAxisY = Input.GetAxis("GamepadLeftY");

		if (leftAxisY > 0.0f)
			MoveThrottle += ort * (leftAxisY * moveInfluence * Vector3.forward);

		if (leftAxisY < 0.0f)
			MoveThrottle += ort * (Mathf.Abs(leftAxisY) * moveInfluence * BackAndSideDampen * Vector3.back);

		if (leftAxisX < 0.0f)
			MoveThrottle += ort * (Mathf.Abs(leftAxisX) * moveInfluence * BackAndSideDampen * Vector3.left);

		if (leftAxisX > 0.0f)
			MoveThrottle += ort * (leftAxisX * moveInfluence * BackAndSideDampen * Vector3.right);
	}

	void UpdateRotation()
	{
		float rotateInfluence = SimulationRate * Time.deltaTime * RotationAmount;

		Vector3 euler = transform.rotation.eulerAngles;

		euler.y += Input.GetAxis("Mouse X") * rotateInfluence * MouseRotationMultiplier;

		float rightAxisX = Input.GetAxis("GamepadRightX");

		euler.y += rightAxisX * rotateInfluence;

		transform.rotation = Quaternion.Euler(euler);
	}
}
