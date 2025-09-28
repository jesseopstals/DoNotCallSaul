using UnityEngine;
using UnityEngine.UI;

public class sprintburst : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] float moveSpeed = 2f;
	[SerializeField] float runSpeed = 5f;
	[SerializeField] float groundDrag = 5f;
	[SerializeField] float airMultiplier = 0.4f;

	[Header("Stamina")]
	[SerializeField] float maxStamina = 100f;
	[SerializeField] float staminaDrain = 20f;   // stamina drained per second
	[SerializeField] float staminaRegen = 10f;   // stamina regenerated per second
	[SerializeField] private float currentStamina;

	[SerializeField] Slider staminaBar;

	[Header("Ground Check")]
	[SerializeField] float playerHeight = 2f;
	[SerializeField] LayerMask whatIsGround;
	bool grounded;

	public Transform orientation;
	float horizontalInput;
	float verticalInput;

	Vector3 moveDirection;
	Rigidbody rb;

	// Track if sprinting was allowed (started at full stamina)
	bool isSprinting = false;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;

		currentStamina = maxStamina;
	}

	void Update()
	{
		grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");

		rb.linearDamping = grounded ? groundDrag : 0;

		HandleStamina();

		if (staminaBar != null)
			staminaBar.value = currentStamina;
	}

	void FixedUpdate()
	{
		MovePlayer();
	}

	private void HandleStamina()
	{
		// Try to start sprint if shift is pressed and stamina is full
		if (Input.GetKey(KeyCode.LeftShift) && verticalInput > 0 && grounded && currentStamina >= maxStamina && !isSprinting)
		{
			isSprinting = true;
		}

		if (isSprinting)
		{
			currentStamina -= staminaDrain * Time.deltaTime;

			if (currentStamina <= 0f)
			{
				currentStamina = 0f;
				isSprinting = false; // force stop sprinting
			}
		}
		else
		{
			// Regen stamina if not sprinting
			currentStamina += staminaRegen * Time.deltaTime;
			currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

			// If fully regenerated, allow sprint again next time shift is pressed
			if (currentStamina >= maxStamina)
				isSprinting = false;
		}
	}

	private void MovePlayer()
	{
		moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

		float speed = isSprinting ? runSpeed : moveSpeed;

		if (grounded)
			rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
		else
			rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
	}
}
