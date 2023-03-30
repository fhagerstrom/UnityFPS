using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Initialize RigidBody component
    Rigidbody rb;
    RaycastHit slopeHit;

    [SerializeField] Transform orientation;

    // BASE MOVEMENT
    [Header("Movement")]
    public float moveSpeed; // "Placeholder" to cahnge speed depnding on movement state.
    public float defaultSpeed = 5.0f;
    public float movementMultiplier = 10.0f;
    public float airMultiplier = 0.4f;

    float horizontalMovement;
    float verticalMovement;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    // KEYBINDS
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    // CROUCHING
    [Header("Crouch")]
    public float crouchSpeed = 3.0f;
    public float crouchYScale = 0.5f;
    private float startYScale;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    public float playerHeight = 2.0f;
    float groundDistance = 0.4f;

    [Header("Drag")]
    float groundDrag = 6.0f;
    float airDrag = 1.0f;


    // Enum for movement state
    public MovementState state;
    public enum MovementState
    {
        walking,
        air,
        crouching
    }

    private void StateHandler()
    {
        // Mode - Walking (Default)
        state = MovementState.walking;
        moveSpeed = defaultSpeed;

        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, (playerHeight / 2) + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);

        PlayerInput();
        ControlDrag();
        StateHandler();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void PlayerInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        // Check if crouch key is held
        if(Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z); // Change y scale to crouch scale, keep x and z.
            rb.AddForce(Vector3.down * 5.0f, ForceMode.Impulse); // If we don't add force downwards, the player will just shrink and "float".
        }

        // If crouch key is released, we reset localScale.
        if(Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void ControlDrag()
    {
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }

        else
        {
            rb.drag = airDrag;
        }
    }

    void MovePlayer()
    {
        // Moving on "flat" surfaces
        if(isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }

        // Moving on slopes
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }

        // Moving in the air
        else if(!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
}
