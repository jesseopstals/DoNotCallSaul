using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
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
	[SerializeField] float staminaCooldown = 3f;
	private float cooldownTimer = 0f;

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
		// Handle cooldown countdown
		if (cooldownTimer > 0f)
		{
			cooldownTimer -= Time.deltaTime;
			isSprinting = false; // can't sprint while exhausted
		}
		else
		{
			// Try to sprint if Shift is held
			if (Input.GetKey(KeyCode.LeftShift) && verticalInput > 0 && grounded && currentStamina > 0)
			{
				isSprinting = true;
			}
			else
			{
				isSprinting = false;
			}
		}

		// If sprinting, drain stamina
		if (isSprinting && rb.linearVelocity.magnitude > 0.1f)
		{
			currentStamina -= staminaDrain * Time.deltaTime;

			if (currentStamina <= 0f)
			{
				currentStamina = 0f;
				isSprinting = false;
				cooldownTimer = staminaCooldown; // start cooldown
			}
		}
		else if (cooldownTimer <= 0f)
		{
			// Regen stamina if not sprinting and not cooling down
			currentStamina += staminaRegen * Time.deltaTime;
			currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
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
