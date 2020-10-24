using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMainController : MonoBehaviour
{
    public SoundManager SoundManager { get; private set; }
    private ControlManager controlManager;
    public InputMaster Controls { get; private set; }

    public float MovementX { get; private set; }
    public bool IsHoldingJumpButton { get; private set; }

    [Header("Dependencies")]

    public Transform groundCheck;
    public Rigidbody2D playerRigidBody;
    public PlayerAnimations playerAnimationsScript;
    public Transform playerSpriteTransform;

    [Header("Movement Variables")]
    public float moveSpeed = 10;
    [Range(0.01f, 1)] public float easingRate = 0.6f;
    public float jumpSpeed = 2;
    public float jumpHeight = 5;
    public float jumpDelay = 0.2f;
    [HideInInspector] public float jumpTimer;
    public float fallMultiplier = 1.5f;

    [Header("Ground Check")]
    public float groundRadius = 1f;
    public LayerMask groundMask;

    [Header("Debug")]
    [SerializeField] private bool debugActivated = true;

    public MainStateMachine StateMachine { get; private set; }
    public string currentStateOutput;

    #region Player Events
    public System.Action hasPerformedJump;
    #endregion

    private void Start()
    {
        SoundManager = SoundManager.Instance;
        controlManager = ControlManager.Instance;
        Controls = controlManager.controls;

        #region Input Handling
        Controls.Player.Jump.performed += _ => jumpTimer = Time.time + jumpDelay;
        Controls.Player.Jump.started += _ => IsHoldingJumpButton = true;
        Controls.Player.Jump.canceled += _ => IsHoldingJumpButton = false;

        Controls.Player.Movement.performed += ctx => MovementX = ctx.ReadValue<Vector2>().x;
        Controls.Player.Movement.canceled += _ => MovementX = 0;

        #endregion

        StateMachine = new MainStateMachine();
        StateMachine.Init(new GroundedState(this, StateMachine));
    }

    private void Update()
    {
        StateMachine.CurrentState.HandleUpdate();
        currentStateOutput = StateMachine.CurrentState.ToString();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.HandleFixedUpdate();
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
        Controls.Player.Jump.performed -= _ => jumpTimer = Time.time + jumpDelay;
        Controls.Player.Jump.started -= _ => IsHoldingJumpButton = true;
        Controls.Player.Jump.canceled -= _ => IsHoldingJumpButton = false;

        Controls.Player.Movement.performed -= ctx => MovementX = ctx.ReadValue<Vector2>().x;
        Controls.Player.Movement.canceled -= _ => MovementX = 0;
    }
}
