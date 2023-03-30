using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Initialize RigidBody component
    Rigidbody rb;

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

    // KEYBINDS
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;

    // CROUCHING
    [Header("Crouch")]
    public float crouchSpeed = 3.0f;
    public float crouchYScale = 0.5f;
    private float startYScale;

    [Header("Ground Check")]
    public float playerHeight = 2.0f;
    bool isGrounded;

    [Header("Drag")]
    float groundDrag = 5.0f;
    float airDrag = 2.0f;


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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        PlayerInput();
        ControlDrag();
        StateHandler();

        isGrounded = Physics.Raycast(transform.position, Vector3.down, (playerHeight / 2) + 0.1f);
        // print(isGrounded);
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
        if(isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }

        else if(!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
}
