using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public LayerMask visionLayerMask;
    public float fieldOfViewAngle = 110f;
    public float visionRange = 10f;
    [SerializeField]private GameObject player;
    private NavMeshAgent agent;
    public Transform[] waypoints; // Patrol waypoints
    private int currentWaypointIndex = 0;
    [SerializeField]private bool pursuingPlayer = false;

    private void Start()
    {

        if (player == null)
        {
            Debug.LogError("Player reference not set in the Inspector.");
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on the enemy GameObject.");
        }

        GoToNextWaypoint();
    }

    public bool HitPlayer { get; set; } // Public property to set the hit status

    private void Update()
    {
        if (HitPlayer)
        {
            Debug.Log("Enemy going back");
            GoToNextWaypoint();
            HitPlayer = false; // Reset the flag
            pursuingPlayer = false; // Stop chasing the player
        }
        else if (pursuingPlayer)
        {
            agent.SetDestination(player.transform.position);
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextWaypoint();
        }
    }

    private void GoToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float angle = Vector3.Angle(directionToPlayer, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, visionRange, visionLayerMask))
                {
                    if (hit.collider.gameObject == player)
                    {
                        pursuingPlayer = true;
                    }
                }
            }
        }
    }
    public void StartChasingPlayer()
    {
        if (player == null)
        {
            Debug.LogError("Player object is null.");
            return;
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is null.");
            return;
        }

        pursuingPlayer = true;
        agent.SetDestination(player.transform.position);
    }
  
    public void StopChasingPlayer()
    {
        pursuingPlayer = false;
        GoToNextWaypoint();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            pursuingPlayer = false;
            GoToNextWaypoint();
        }
    }
}
