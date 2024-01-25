using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input Settings")]
    private PlayerInputs playerActionsAsset;
    private InputAction move;

    [Header("Movement Settings")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [Header("Physics Settings")]
    [SerializeField] private float stoppingFactor = 5f;
    [SerializeField] private float customGravityScale = 2f;
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public bool bIsJumping;

    [Header("Components")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator animator;

    [Header("VFX")]
    [SerializeField] private GameObject footstepVFXPrefab;
    [SerializeField] private Transform leftFootTransform;
    [SerializeField] private Transform rightFootTransform;
    [SerializeField] private float vfxLifetime = 1f;

    private enum PlayerState { Idle, Walking, Jumping, Falling }
    private PlayerState currentState = PlayerState.Idle;

    private void Awake()
    {
        InitializeComponents();
        LockCursor();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        playerActionsAsset = new PlayerInputs();
        animator = GetComponent<Animator>();
        playerCamera = Camera.main;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleEscapeKey();
        HandleJumpInput();
    }

    private void HandleEscapeKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HandleJumpInput()
    {
        if (playerActionsAsset.Player.Jump.triggered && IsGrounded())
        {
            DoJump();
        }
    }

    private void FixedUpdate()
    {
        UpdateState();
        ApplyMovementLogic();
        ApplyCustomGravity();
    }

    [SerializeField] public bool isFalling; // The new boolean to track the falling state


    private void UpdateState()
    {
        // Check if the player is falling
        isFalling = rb.velocity.y < 0 && !IsGrounded();


        switch (currentState)
        {
            case PlayerState.Idle:
                if (IsMoving())
                    currentState = PlayerState.Walking;
                if (isFalling)
                    currentState = PlayerState.Falling;
                break;

            case PlayerState.Walking:
                if (!IsMoving())
                    currentState = PlayerState.Idle;
                else if (isFalling)
                    currentState = PlayerState.Falling;
                break;

            case PlayerState.Jumping:
                if (IsGrounded())
                    currentState = IsMoving() ? PlayerState.Walking : PlayerState.Idle;
                else if (isFalling)
                    currentState = PlayerState.Falling;
                break;

            case PlayerState.Falling:
                if (IsGrounded())
                    currentState = IsMoving() ? PlayerState.Walking : PlayerState.Idle;
                break;
        }

        if (isFalling)
        {
            currentState = PlayerState.Falling;
        }
        UpdateAnimatorParameters();
        ApplyMovementLogic();
    }


    private void ApplyMovementLogic()
    {
        ApplyMovement();

        if (currentState == PlayerState.Idle)
        {
            Decelerate();
        }

        // Apply custom gravity if needed
        if (!IsGrounded())
        {
            ApplyCustomGravity();
        }
    }
    private void ApplyMovement()
    {
        Vector2 inputVector = move.ReadValue<Vector2>();
        Vector3 forwardMovement = GetCameraForward() * inputVector.y;
        Vector3 rightMovement = GetCameraRight() * inputVector.x;

        Vector3 desiredDirection = (forwardMovement + rightMovement).normalized;
        float currentMovementForce = currentState == PlayerState.Falling ? movementForce * 0.5f : movementForce;

        forceDirection += desiredDirection * currentMovementForce;

        if (desiredDirection.sqrMagnitude > 0.01f)
        {
            rb.AddForce(forceDirection, ForceMode.Impulse);

            // Update rotation
            UpdateRotation(desiredDirection);
        }
        forceDirection = Vector3.zero;

        ClampVelocity();
    }

    private void UpdateRotation(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void ClampVelocity()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }
    }

    

    private void Decelerate()
    {
        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, rb.velocity.y, 0), Time.fixedDeltaTime * stoppingFactor);
    }

    private void ApplyCustomGravity()
    {
        if (!IsGrounded() && rb.velocity.y < 0)
        {
            rb.AddForce(Physics.gravity * (customGravityScale - 1) * rb.mass);
        }
    }

    private void DoJump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            currentState = PlayerState.Jumping;
            animator.SetTrigger("Jump");
            bIsJumping= true;
        }
    }

    private bool IsGrounded()
    {
        Vector3 feetPosition = transform.position + Vector3.up * 0.1f;
        bool isGrounded = Physics.Raycast(feetPosition, Vector3.down, groundCheckDistance, groundLayer);
        bIsJumping = false;
        return isGrounded;
    }

    private bool IsMoving()
    {
        return move.ReadValue<Vector2>().sqrMagnitude > 0.1f;
    }

    private void UpdateAnimatorParameters()
    {
        animator.SetBool("isGrounded", IsGrounded());
        animator.SetBool("isJumping", currentState == PlayerState.Jumping);
        animator.SetBool("isFalling", rb.velocity.y < 0 && !IsGrounded());
        animator.SetFloat("speed", rb.velocity.magnitude / maxSpeed);
    }

    private void OnEnable()
    {
        playerActionsAsset.Player.Jump.started += _ => DoJump();
        move = playerActionsAsset.Player.Move;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        playerActionsAsset.Player.Jump.started -= _ => DoJump();
        playerActionsAsset.Player.Disable();
    }
    public void CreateLeftFootstepVFX()
    {
        if (footstepVFXPrefab != null && IsGrounded())
        {
            GameObject vfx = Instantiate(footstepVFXPrefab, leftFootTransform.position, Quaternion.identity);
            Destroy(vfx, vfxLifetime);
        }
    }

    public void CreateRightFootstepVFX()
    {
        if (footstepVFXPrefab != null && IsGrounded())
        {
            GameObject vfx = Instantiate(footstepVFXPrefab, rightFootTransform.position, Quaternion.identity);
            Destroy(vfx, vfxLifetime);
        }
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0; // Remove the vertical component
        return forward.normalized;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0; // Remove the vertical component
        return right.normalized;
    }

   
}
