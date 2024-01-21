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
    private Camera playerCamera;
    [SerializeField]private Animator animator;

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
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
            return true;
        else
            return false;
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
        }
    }

    private void ApplyMovement()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

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
            forceDirection += Vector3.up * jumpForce;
            currentState = PlayerState.Jumping;
        }
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
