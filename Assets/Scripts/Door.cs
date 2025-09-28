using UnityEngine;

public class Door : MonoBehaviour
{
	private Animator animator;
	public Transform player;
	public Transform enemy;
	public float detectionRadius = 3f;
	
	public AudioClip openClip;
    public AudioClip closeClip;

    private AudioSource audioSource;

	void Start()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		Transform target = GetClosestTarget();
		if (target != null)
		{
			animator.SetBool("Open", true);

			// figure out which side target is on
			Vector3 toTarget = transform.InverseTransformPoint(target.position);
			if (toTarget.z > 0)
			{
				animator.SetFloat("Direction", 1); // front
			}
			else
			{
				animator.SetFloat("Direction", -1); // back

			}
		}
		else
		{
			animator.SetBool("Open", false);
		}
	}

	Transform GetClosestTarget()
	{
		float closest = detectionRadius;
		Transform closestTarget = null;

		float playerDist = Vector3.Distance(transform.position, player.position);
		if (playerDist < closest) { closest = playerDist; closestTarget = player; }

		float enemyDist = Vector3.Distance(transform.position, enemy.position);
		if (enemyDist < closest) { closest = enemyDist; closestTarget = enemy; }

		return closestTarget;
	}

	public void PlayDoorOpenSound()
    {
        audioSource.PlayOneShot(openClip);
    }

    public void PlayDoorCloseSound()
    {
        audioSource.PlayOneShot(closeClip);
    }
}
