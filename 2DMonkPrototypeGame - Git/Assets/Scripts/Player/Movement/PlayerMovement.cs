using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    private InputMaster controls;
    
    private float movementX;
    private float easingMovementX;
    private bool isHoldingJumpButton;

    [Header("Object Assignments")]

    public Transform groundCheck;
    
    [Header("Movement Variables")]
    public float moveSpeed = 10;
    [Range(0.01f, 1)] public float easingRate = 0.6f;
    public float jumpHeight = 5;
    public float fallMultiplier = 1.5f;

    private Rigidbody2D rb;

    [Header("Ground Check")]
    public float groundRadius = 0.3f;
    public LayerMask groundMask;
    private bool isGrounded;

    [Header("Debug")]
    public bool debugActivated = true;


    private void Awake()
    {
        controls = new InputMaster();
        rb = GetComponent<Rigidbody2D>();

        #region Input Handling
        controls.Player.Jump.performed += OnJump;
        controls.Player.Jump.started += OnPressingJump;
        controls.Player.Jump.canceled += OnReleasingJump;

        controls.Player.Movement.performed += OnMove;
        controls.Player.Movement.canceled += OnCancelingMove;
        #endregion
    }

    

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);

        easingMovementX = Mathf.Lerp(easingMovementX, movementX, easingRate);

        if(easingMovementX < 0.01f && easingMovementX > -0.01f)
        {
            easingMovementX = 0;
        }
        if (easingMovementX > 0.99f)
        {
            easingMovementX = 1;
        }
        if (easingMovementX < -0.99f)
        {
            easingMovementX = -1;
        }
    }
    private void FixedUpdate()
    {
        
        if (rb.velocity.y < 0 || rb.velocity.y > 0 && !isHoldingJumpButton)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        
        if(isGrounded && rb.velocity.y < -0.8f)
        {
            rb.velocity = Vector2.up * 0;
        }
        rb.velocity = new Vector2(easingMovementX * moveSpeed, rb.velocity.y);

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(isGrounded)
            rb.velocity = Vector2.up * Mathf.Sqrt(jumpHeight * -2 * Physics2D.gravity.y);
    }

    public void OnPressingJump(InputAction.CallbackContext context)
    {
        isHoldingJumpButton = true;
    }

    public void OnReleasingJump(InputAction.CallbackContext context)
    {
        isHoldingJumpButton = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementX = context.ReadValue<Vector2>().x;
    }

    private void OnCancelingMove(InputAction.CallbackContext context)
    {
        movementX = 0;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void OnDrawGizmos()
    {
        if (debugActivated)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
        
    }
}
