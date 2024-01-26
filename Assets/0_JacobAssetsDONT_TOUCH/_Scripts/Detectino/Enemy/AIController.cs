using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    Waypoint waypoint;
    private float waypointWaitTimer; // Timer for waiting at a waypoint
    private bool isWaiting = false;

    [Header("Movement")]
    public NavMeshAgent agent;
    public Waypoint[] waypoints;
    private int waypointIndex = 0;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    [Header("Sight")]
    public float visionRadius = 20f;
    public float fieldOfViewAngle = 45f;

    [Header("Attack")]
    public float attackDistance = 5f;
    public float damageAmount = 10f;

    [Header("Animation")]
    public Animator animator;

    private GameObject player;
    private bool isChasing = false;

    private EnemyCollision enemyCollisionScript;

    public AudioSource audioSource; // Assign this in the Unity Inspector
    public AudioClip soundEffect; // Assign the sound effect clip in the Unity Inspector
    public AudioClip walkSound;
    public AudioClip deathSound;
    public AudioClip attackSound;

    [SerializeField] private GameObject leftFootVFXPrefab;
    [SerializeField] private GameObject rightFootVFXPrefab;
    [SerializeField] private Transform leftFootTransform;
    [SerializeField] private Transform rightFootTransform;
    [SerializeField] private float vfxLifetime = 2f;

    [SerializeField] private bool bAttack;


    private RagdollController playerController;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        GoToNextWaypoint();
        waypointWaitTimer = waypoints[waypointIndex].waitTime;

        enemyCollisionScript = GetComponent<EnemyCollision>();

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<RagdollController>();
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= visionRadius && CanSeePlayer())
        {
            if (!isChasing)
            {
                StartChasingPlayer();
            }
            else
            {
                agent.SetDestination(player.transform.position); 
            }
        }
        else if (distanceToPlayer <= attackDistance)
        {
            AttackPlayer();
        }
        else
        {
            if (isChasing)
            {
                GoToNextWaypoint();
                isChasing = false;
            }

            if (!isWaiting && !agent.pathPending && agent.remainingDistance < 0.5f)
            {
                StartWaitingAtWaypoint();
            }

            if (isWaiting)
            {
                waypointWaitTimer -= Time.deltaTime;
                if (waypointWaitTimer <= 0)
                {
                    isWaiting = false;
                    GoToNextWaypoint();
                }
            }
        }

        UpdateAnimator();
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);
        return angle < fieldOfViewAngle;
    }
    private void StartWaitingAtWaypoint()
    {
        isWaiting = true;
        waypointWaitTimer = waypoints[waypointIndex].waitTime;

        Quaternion lookRotation = Quaternion.Euler(waypoints[waypointIndex].lookEulers);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }

    private void StartChasingPlayer()
    {
        LookAtPlayer();
        PlaySpottedAnimation();
        agent.speed = chaseSpeed;
        agent.SetDestination(player.transform.position);
        agent.isStopped = false;
        isChasing = true;
    }
    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0; 
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * patrolSpeed);
    }
    public void PlaySpottedAnimation()
    {
        if (animator == null)
        {
            Debug.LogError("Animator not found on the enemy.");
            return;
        }

        //animator.SetTrigger("SpottedPlayer");
        agent.isStopped = true;
    }
    private void AttackPlayer()
    {
        if(bAttack)
        {
            return;
        }
        if (playerController.bPlayerDead & playerController.isRagDollForceOn)
        {
            return;
        }
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackDistance)
        {
            bAttack = true;
            animator.SetTrigger("Attack");
            PlayAttackSound();
            agent.isStopped = true; 
        }else
        {
            bAttack = false;
        }
    }
    private void GoToNextWaypoint()
    {
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
        agent.destination = waypoints[waypointIndex].targetPosition.position;
        waypointWaitTimer = waypoints[waypointIndex].waitTime;
        isWaiting = false; 
        agent.speed = patrolSpeed;
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("speed", agent.velocity.magnitude);
    }
    public void CreateLeftFootstepVFX()
    {
        if (leftFootVFXPrefab != null)
        {
            GameObject vfx = Instantiate(leftFootVFXPrefab, leftFootTransform.position, Quaternion.identity);
            PlayWalkSound();
            Destroy(vfx, vfxLifetime);
        }
    }

    public void CreateRightFootstepVFX()
    {
        if (rightFootVFXPrefab != null)
        {
            GameObject vfx = Instantiate(rightFootVFXPrefab, rightFootTransform.position, Quaternion.identity);
            PlayWalkSound();
            Destroy(vfx, vfxLifetime);
        }
    }
    public void PlaySoundEffect()
    {
        audioSource.PlayOneShot(soundEffect);
    }
    public void PlayWalkSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(walkSound);
        }
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
        Debug.Log("playing enemy death sound");
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
        Debug.Log("playing attack sound");
    }
}

[System.Serializable]
public class Waypoint
{
    public Transform targetPosition;
    public Vector3 lookEulers;
    public float waitTime;
}
