using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    private ControlManager controlManager;
    private InputMaster controls;

    private float movementX;
    private float easingMovementX;
    private bool isHoldingJumpButton;

    [Header("Dependencies")]

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerAnimations playerAnimationsScript;
    [SerializeField] private Transform playerSpriteTransform;

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] [Range(0.01f, 1)] private float easingRate = 0.6f;
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private float fallMultiplier = 1.5f;

    [Header("Ground Check")]
    [SerializeField] private float groundRadius = 1f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;

    [Header("Debug")]
    [SerializeField] private bool debugActivated = true;

    public System.Action hasPerformedJump;

    private void Start()
    {
        controlManager = ControlManager.Instance;
        controls = controlManager.controls;

        #region Input Handling
        controls.Player.Jump.performed += OnJump;
        controls.Player.Jump.started += _ => isHoldingJumpButton = true;
        controls.Player.Jump.canceled += _ => isHoldingJumpButton = false;

        controls.Player.Movement.performed += ctx => movementX = ctx.ReadValue<Vector2>().x;
        controls.Player.Movement.canceled += _ => movementX = 0;

        #endregion
    }

    private void Update()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);

        easingMovementX = Mathf.Lerp(easingMovementX, movementX, easingRate);

        easingMovementX = ClampMovement(easingMovementX);

        if (isGrounded && playerAnimationsScript.CurrentState != "player_jump")
        {
            if (movementX != 0)
            {
                playerAnimationsScript.ChangeAnimationState("player_walk");
                
            }
            else
            {
                playerAnimationsScript.ChangeAnimationState("player_idle");
            }
        }
        else if(rb.velocity.y < 0)
        {
            playerAnimationsScript.ChangeAnimationState("player_fall");
        }

        if (movementX > 0)
        {
            playerSpriteTransform.localScale = new Vector2(1, 1);
        }
        else if (movementX < 0)
        {
            playerSpriteTransform.localScale = new Vector2(-1, 1);
        }

    }

    private void FixedUpdate()
    {

        if (isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = Vector2.up * Physics.gravity.y;

        }
        else if (rb.velocity.y < 0 || rb.velocity.y > 0 && !isHoldingJumpButton)
        {
            
            rb.velocity += Vector2.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
            
        }

        float tempSpeed = easingMovementX * moveSpeed;
        
        rb.velocity = new Vector2(tempSpeed, rb.velocity.y);

    }

    private float ClampMovement(float value)
    {
        if (value < 0.01f && value > -0.01f)
        {
            value = 0;
        }
        if(value > 0.99f)
        {
            value = 1;
        }
        if (value < -0.99f)
        {
            value = -1;
        }
        return value;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            isGrounded = false;
            rb.velocity = Vector2.up * Mathf.Sqrt(jumpHeight * -2 * Physics2D.gravity.y);
            hasPerformedJump?.Invoke();
            playerAnimationsScript.ChangeAnimationState("player_jump");
        }
            
    }


    private void OnDrawGizmos()
    {
        if (debugActivated)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
            
        }
        
    }

    private void OnDestroy()
    {
        controls.Player.Jump.performed -= OnJump;
        controls.Player.Jump.started -= _ => isHoldingJumpButton = true;
        controls.Player.Jump.canceled -= _ => isHoldingJumpButton = false;

        controls.Player.Movement.performed -= ctx => movementX = ctx.ReadValue<Vector2>().x;
        controls.Player.Movement.canceled -= _ => movementX = 0;
    }
}
