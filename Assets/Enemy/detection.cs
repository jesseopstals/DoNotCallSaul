using UnityEngine;
using UnityEngine.AI; // Needed for NavMeshAgent

public class EnemyDetection : MonoBehaviour
{
	[SerializeField] GameObject player;
	[SerializeField] float FOV = 60f;
	[SerializeField] float viewRadius = 10f;
	[SerializeField] float chaseDuration = 3f;
	[SerializeField] float viewHeight = 1.6f;
	[SerializeField] LayerMask obstructionMask;

	[Header("Movement Speed")]
	[SerializeField] float speed = 1f;
	[SerializeField] float chaseSpeed = 3f;

	[Header("Audio")]
	[SerializeField] AudioSource chaseAudio;

	private bool isChasing = false;
	private float chaseTimer = 0f;

	private NavMeshAgent agent;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
	}

	void Update()
	{
		bool playerVisible = CanSeePlayer();

		if (playerVisible)
		{
			isChasing = true;
			agent.speed = chaseSpeed;
			chaseTimer = chaseDuration;
		}
		else if (isChasing)
		{
			chaseTimer -= Time.deltaTime;
			if (chaseTimer <= 0f)
			{
				isChasing = false;
				agent.ResetPath(); // stop moving
				agent.speed = speed;
				if (chaseAudio != null && chaseAudio.isPlaying)
					chaseAudio.Stop();
				Debug.Log("Chase ended.");
			}
		}

		if (isChasing)
		{
			agent.SetDestination(player.transform.position);
			if (chaseAudio != null && !chaseAudio.isPlaying)
				chaseAudio.Play();
			Debug.Log("Chase started!");
		}
	}

	bool CanSeePlayer()
	{
		Vector3 eyePos = transform.position + Vector3.up * viewHeight;
		Vector3 dirToPlayer = (player.transform.position - eyePos).normalized;
		float distanceToPlayer = Vector3.Distance(eyePos, player.transform.position);


		// Forward vision range (green)
		Debug.DrawRay(eyePos, transform.forward * viewRadius, Color.green);

		// FOV left and right (purple)
		Quaternion leftRotation = Quaternion.AngleAxis(-FOV / 2f, Vector3.up);
		Quaternion rightRotation = Quaternion.AngleAxis(FOV / 2f, Vector3.up);

		Vector3 leftDir = leftRotation * transform.forward;
		Vector3 rightDir = rightRotation * transform.forward;

		Debug.DrawRay(eyePos, leftDir * viewRadius, Color.purple);  // Left boundary
		Debug.DrawRay(eyePos, rightDir * viewRadius, Color.purple); // Right boundary

		if (distanceToPlayer > viewRadius) return false;

		// Direct line to the player for debugging purposes (blue)
		Debug.DrawRay(eyePos, dirToPlayer * distanceToPlayer, Color.blue);
		
		// Angle cone FOV (yellow)
		Debug.DrawRay(eyePos, dirToPlayer * viewRadius, Color.yellow);

		float angle = Vector3.Angle(transform.forward, dirToPlayer);
		if (angle > FOV / 2f) return false;


		if (Physics.Raycast(eyePos, dirToPlayer, out RaycastHit hit, viewRadius))
		{
			Debug.DrawRay(eyePos, dirToPlayer * hit.distance, Color.red); // Stops at first hit
			return hit.collider.gameObject == player;
		}

		return false;
	}
}
