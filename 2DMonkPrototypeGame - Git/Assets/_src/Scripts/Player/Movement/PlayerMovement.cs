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
    [SerializeField] private Rigidbody2D playerRigidBody;
    [SerializeField] private PlayerAnimations playerAnimationsScript;
    [SerializeField] private Transform playerSpriteTransform;

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] [Range(0.01f, 1)] private float easingRate = 0.6f;
    [SerializeField] private float jumpSpeed = 2;
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private float jumpDelay = 0.2f;
    private float jumpTimer;
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
        controls.Player.Jump.performed += _ => jumpTimer = Time.time + jumpDelay;
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
        else if(playerRigidBody.velocity.y < 0)
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
        
        if (isGrounded && jumpTimer > Time.time)
        {
            Jump();
        }

        if (isGrounded && playerRigidBody.velocity.y < 0)
        {
            playerRigidBody.velocity = Vector2.up * Physics2D.gravity.y;
        }
        else if (playerRigidBody.velocity.y < 0 || playerRigidBody.velocity.y > 0 && !isHoldingJumpButton)
        {
            
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
            
        }
        

        float tempSpeed = easingMovementX * moveSpeed;
        
        playerRigidBody.velocity = new Vector2(tempSpeed, playerRigidBody.velocity.y);

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

    private void Jump()
    {
        isGrounded = false;
        playerRigidBody.velocity = Vector2.up * Mathf.Sqrt(jumpHeight * -2 * Physics2D.gravity.y * playerRigidBody.gravityScale);
        hasPerformedJump?.Invoke();
        playerAnimationsScript.ChangeAnimationState("player_jump");
        jumpTimer = 0;
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
        controls.Player.Jump.performed -= _ => jumpTimer = Time.time + jumpDelay;
        controls.Player.Jump.started -= _ => isHoldingJumpButton = true;
        controls.Player.Jump.canceled -= _ => isHoldingJumpButton = false;

        controls.Player.Movement.performed -= ctx => movementX = ctx.ReadValue<Vector2>().x;
        controls.Player.Movement.canceled -= _ => movementX = 0;
    }
}
