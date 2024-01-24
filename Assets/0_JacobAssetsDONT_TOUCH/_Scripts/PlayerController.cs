using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    //input fields
    private PlayerInputs playerActionsAsset;
    private InputAction move;

    //movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private float stoppingFactor = 5f;

    [SerializeField]
    private float customGravityScale = 2f;

    [SerializeField]
    private float groundCheckDistance = 0.3f; 
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]private Animator animator;

    [SerializeField] private GameObject footstepVFXPrefab;
    [SerializeField] private Transform leftFootTransform;
    [SerializeField] private Transform rightFootTransform;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new PlayerInputs();
        animator = this.GetComponent<Animator>();
        playerCamera = Camera.main;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (playerActionsAsset.Player.Jump.triggered && IsGrounded())
        {
            DoJump(new InputAction.CallbackContext());
        }
    }
    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }


    private bool IsGrounded()
    {
        // Check if the player's feet position is close to the ground
        Vector3 feetPosition = transform.position + Vector3.up * 0.1f; // Adjust the height as needed
        bool isGrounded = Physics.Raycast(feetPosition, Vector3.down, groundCheckDistance, groundLayer);
        animator.SetBool("isGrounded", isGrounded); // Update animator parameter
        return isGrounded;
    }

    //private void DoAttack(InputAction.CallbackContext obj)
    //{
    //    animator.SetTrigger("attack");
    //}

    [SerializeField]private enum PlayerState
    {
        Idle,
        Walking,
        Jumping
    }

    [SerializeField]private PlayerState currentState = PlayerState.Idle;


    private void FixedUpdate()
    {
       
           

        switch (currentState)
        {
            case PlayerState.Idle:
                HandleIdleState();
                break;
            case PlayerState.Walking:
                HandleWalkingState();
                break;
            case PlayerState.Jumping:
                HandleJumpingState();
                break;
        }

        bool isMoving = IsMoving();
        bool isGrounded = IsGrounded();

        if (currentState == PlayerState.Jumping && isGrounded)
        {
            currentState = isMoving ? PlayerState.Walking : PlayerState.Idle;
        }
        else if (isMoving)
        {
            currentState = PlayerState.Walking;
        }
        else if (!isMoving && isGrounded)
        {
            currentState = PlayerState.Idle;
        }
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isJumping", currentState == PlayerState.Jumping);
        animator.SetBool("isFalling", rb.velocity.y < 0 && !IsGrounded());
        ApplyMovement();
        if (!IsGrounded() && rb.velocity.y < 0)
        {
            rb.AddForce(Physics.gravity * (customGravityScale - 1) * rb.mass);
        }
        UpdateAnimatorParameters();
    }
    public void CreateLeftFootstepVFX()
    {
        if (footstepVFXPrefab != null && IsGrounded())
        {
            Instantiate(footstepVFXPrefab, leftFootTransform.position, Quaternion.identity);
            Debug.Log("left foot vfx");
        }
    }

    public void CreateRightFootstepVFX()
    {
        if (footstepVFXPrefab != null && IsGrounded())
        {
            Instantiate(footstepVFXPrefab, rightFootTransform.position, Quaternion.identity);
            Debug.Log("right foot vfx");
        }
    }

    private void HandleIdleState()
    {
        if (IsMoving())
        {
            currentState = PlayerState.Walking;
        }
        else if (!IsGrounded())
        {
            currentState = PlayerState.Jumping;
        }
        // Additional idle logic here
    }

    private void HandleWalkingState()
    {
        ApplyMovement();

        if (!IsMoving())
        {
            currentState = PlayerState.Idle;
        }
        else if (!IsGrounded())
        {
            currentState = PlayerState.Jumping;
        }
    }

    private void HandleJumpingState()
    {
        ApplyMovement();

        if (IsGrounded())
        {
            currentState = IsMoving() ? PlayerState.Walking : PlayerState.Idle;
            // Reset the jumping Animator parameter when landing
            animator.SetBool("isJumping", false);
        }
    }

    private void ApplyMovement()
    {
        if (IsMoving())
        {
            // Apply movement forces only when there is input
            forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
            forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

            rb.AddForce(forceDirection, ForceMode.Impulse);
            forceDirection = Vector3.zero;
        }
        else
        {
            // Gradually reduce the velocity when there is no input
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, rb.velocity.y, 0), Time.fixedDeltaTime * stoppingFactor);

        }

        if (rb.velocity.y < 0f)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }
        if (rb.velocity.y < 0 && !IsGrounded())
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }

        // Set the Animator parameter for walking
        animator.SetFloat("speed", rb.velocity.magnitude / maxSpeed);
        LookAt();
    }

    private bool IsMoving()
    {
        return move.ReadValue<Vector2>().sqrMagnitude > 0.1f;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            currentState = PlayerState.Jumping;
            // Set the Animator parameter for jumping
            animator.SetBool("isJumping", true);
        }
    }
    private void UpdateAnimatorParameters()
    {
        animator.SetFloat("speed", rb.velocity.magnitude / maxSpeed);
        animator.SetBool("isJumping", currentState == PlayerState.Jumping);
        animator.SetBool("isFalling", rb.velocity.y < 0 && !IsGrounded());
        // isGrounded is already set within the IsGrounded() method
    }
    private void OnEnable()
    {
        playerActionsAsset.Player.Jump.started += DoJump;
        move = playerActionsAsset.Player.Move;
        playerActionsAsset.Player.Enable();
    }
    private void OnDisable()
    {
        playerActionsAsset.Player.Jump.started -= DoJump;
        playerActionsAsset.Player.Disable();
    }
}
