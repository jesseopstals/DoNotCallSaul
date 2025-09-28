using UnityEngine;
using UnityEngine.AI;

public class AiPatrol : MonoBehaviour
{
    public Transform[] waypoints; // Array to hold references to waypoints
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length == 0)
        {
            enabled = false;
        }
        else
        {
            ShuffleWaypoints();
            SetDestination();
        }
    }

    void SetDestination()
    {
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void ShuffleWaypoints() 
    { 
        for (int i = 0; i < waypoints.Length; i++)
        {
            int randomIndex = Random.Range(i, waypoints.Length);
            Transform temp = waypoints[i];
            waypoints[i] = waypoints[randomIndex];
            waypoints[randomIndex] = temp;
        }
    }

    void Update()
    {
        // Check if reached the current waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            // Move to the next waypoint
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                // Loop back to the beginning if reached the last waypoint
                currentWaypointIndex = 0;
            }
            SetDestination();
        }
    }
}