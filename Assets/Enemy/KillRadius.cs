using UnityEngine;
using UnityEngine.SceneManagement;

public class KillRadius : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Kill Radius")]
    [SerializeField] float killRadius = 1.5f;
    [SerializeField] float heightOffset = 0.8f;
    [SerializeField] GameObject player;
    [SerializeField] LayerMask obstructionMask;
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        Vector3 killArea = transform.position + Vector3.up * heightOffset;
        float distance = Vector3.Distance(killArea, player.transform.position);

        if(distance <= killRadius)
        {
            if (HasLineOfSight(killArea, player.transform.position))
            {
				KillPlayer();
			}   
        }
    }

    bool HasLineOfSight(Vector3 start, Vector3 target)
    {
        Vector3 dir = target - start;
        if (Physics.Raycast(start, dir.normalized, out RaycastHit hit, dir.magnitude, ~0))
        {
            return hit.collider.gameObject == player;
        }
        return false;
    }

	void KillPlayer()
	{
		SceneManager.LoadScene("Game over");
		Debug.Log("Player is dead!");
	}

    void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.red;
		Vector3 killCenter = transform.position + Vector3.up * heightOffset;
		Gizmos.DrawWireSphere(killCenter, killRadius);
	}
}
