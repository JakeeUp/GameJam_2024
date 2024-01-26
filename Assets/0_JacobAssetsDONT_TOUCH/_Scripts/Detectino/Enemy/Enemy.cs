using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    AIController controller;
    public LayerMask visionLayerMask;
    public float fieldOfViewAngle = 110f;
    public float visionRange = 10f;
    [SerializeField]private GameObject player;
    private NavMeshAgent agent;
    public Transform[] waypoints; 
    private int currentWaypointIndex = 0;
    [SerializeField]private bool pursuingPlayer = false;

    private Animator animator;
    [SerializeField] private GameObject leftFootVFXPrefab; 
    [SerializeField] private GameObject rightFootVFXPrefab; 
    [SerializeField] private Transform leftFootTransform;
    [SerializeField] private Transform rightFootTransform;
    [SerializeField] private float vfxLifetime = 2f;

    [SerializeField] private GameObject stompVFXPrefab;

    private void Awake()
    {
        controller= GetComponent<AIController>();
    }

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

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the enemy GameObject.");
        }
        //GoToNextWaypoint();
    }

    public bool HitPlayer { get; set; } 

    private void Update()
    {
        if (HitPlayer)
        {
            Debug.Log("Enemy going back");
            //GoToNextWaypoint();
            HitPlayer = false;
            pursuingPlayer = false; 
        }
        else if (pursuingPlayer)
        {
            agent.SetDestination(player.transform.position);
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            //GoToNextWaypoint();
        }
        animator.SetFloat("speed", agent.velocity.magnitude);

        if (pursuingPlayer && !animator.GetCurrentAnimatorStateInfo(0).IsName("SpottedPlayer"))
        {
            ResumeMovement();
            agent.SetDestination(player.transform.position);
        }
    }
    public void PlaySpottedAnimation()
    {
        if (animator == null)
        {
            Debug.LogError("Animator not found on the enemy.");
            return;
        }

        animator.SetTrigger("SpottedPlayer");
        agent.isStopped = true;
    }

    public void OnSpottedAnimationComplete()
    {
        ResumeMovement();
        StartChasingPlayer();
    }

    public void ResumeMovement()
    {
        animator.ResetTrigger("SpottedPlayer");
        agent.isStopped = false; 
    }

    public void StartChasingPlayer()
    {
        pursuingPlayer = true;
        ResumeMovement(); // Ensure movement is resumed when starting to chase
        agent.SetDestination(player.transform.position);
    }

    //public void CreateLeftFootstepVFX()
    //{
    //    if (leftFootVFXPrefab != null)
    //    {
    //        GameObject vfx = Instantiate(leftFootVFXPrefab, leftFootTransform.position, Quaternion.identity);
    //        Destroy(vfx, vfxLifetime); 
    //    }
    //}

    //public void CreateRightFootstepVFX()
    //{
    //    if (rightFootVFXPrefab != null)
    //    {
    //        GameObject vfx = Instantiate(rightFootVFXPrefab, rightFootTransform.position, Quaternion.identity);
    //        Destroy(vfx, vfxLifetime); 
    //    }
    //}

    //private void GoToNextWaypoint()
    //{
    //    if (waypoints.Length == 0)
    //        return;

    //    agent.destination = waypoints[currentWaypointIndex].position;
    //    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    //}

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
    //public void StartChasingPlayer()
    //{
    //    if (player == null)
    //    {
    //        Debug.LogError("Player object is null.");
    //        return;
    //    }

    //    if (agent == null)
    //    {
    //        Debug.LogError("NavMeshAgent is null.");
    //        return;
    //    }

    //    pursuingPlayer = true;
    //    agent.SetDestination(player.transform.position);
    //}

    public void StopChasingPlayer()
    {
        pursuingPlayer = false;
       // GoToNextWaypoint();
    }
    public void StompEnemy()
    {
        // Instantiate the stomp VFX at the enemy's position
        if (stompVFXPrefab != null)
        {
            Instantiate(stompVFXPrefab, transform.position, Quaternion.identity);
        }
        AudioManager.instance.PlayEnemyDeathSound();

        Debug.Log("destroy Enemy");
        //controller.PlayDeathSound();
        // Destroy the enemy GameObject
        Destroy(gameObject);
    }

   

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            pursuingPlayer = false;
           // GoToNextWaypoint();
        }
    }
}
