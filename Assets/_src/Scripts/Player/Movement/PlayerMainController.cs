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
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isHittingHead;
    public float MovementX { get; private set; }
    public float MovementY { get; private set; }
    public bool IsHoldingJumpButton { get; private set; }

    [Header("Dependencies")]

    public Transform groundCheck;
    public Transform ceilingCheck;
    public Rigidbody2D playerRigidBody;
    public CapsuleCollider2D playerMainCollider;
    public AnimationsState playerAnimationsScript;
    public Transform playerSpriteTransform;
    public HitCheck hitBoxCheck;
    public PlayerAttackMoveList playerMoveList;
    public AfterImageEffectPool afterImageEffectPool;

    [Header("Movement Variables")]
    public float standingMoveSpeed = 10;
    public float crouchingMoveSpeed = 5;
    [Range(0.01f, 1)] public float easingRate = 0.6f;
    public float jumpSpeed = 2;
    public float jumpHeight = 5;
    public float airborneJumpDelay = 0.2f;
    public float groundedJumpDelay = 0.2f;
    [HideInInspector] public float airborneJumpTimer;
    [HideInInspector] public float groundedJumpTimer;
    public float fallMultiplier = 1.5f;

    [Header("Ground Check")]
    public float groundCheckRadius = 1f;
    public LayerMask groundMask;

    [Header("Ceiling Check")]
    public float ceilingCheckRadius = 1f;
    public LayerMask ceilingMask;

    [Header("Debug")]
    [SerializeField] private bool debugActivated = true;
    public MainStateMachine StateMachine { get; private set; }
    public string currentStateOutput;

    #region Input Events
    public Action<InputAction.CallbackContext> kickAction;
    public Action<InputAction.CallbackContext> punchAction;
    #endregion
    #region Player Events
    public System.Action hasPerformedJump;
    #endregion

    private void Start()
    {
        SoundManager = SoundManager.Instance;
        controlManager = ControlManager.Instance;
        Controls = controlManager.controls;

        #region Input Handling
        Controls.Player.Jump.performed += _ => airborneJumpTimer = Time.time + airborneJumpDelay;
        Controls.Player.Jump.started += _ => IsHoldingJumpButton = true;
        Controls.Player.Jump.canceled += _ => IsHoldingJumpButton = false;

        Controls.Player.Movement.performed += ctx =>
        {
            MovementX = ctx.ReadValue<Vector2>().x;
            MovementY = ctx.ReadValue<Vector2>().y;
        };
        Controls.Player.Movement.canceled += _ =>
        {
            MovementX = 0;
            MovementY = 0;
        };

        Controls.Player.PunchKick.performed += _ => Debug.Log("Performed:");

        #endregion

        InputSubscribe();
        StateMachine = new MainStateMachine();
        StateMachine.Init(new GroundedState(this, StateMachine));

        StateMachine.onStateChanged += InputUnsubscribe;
        
    }
    public void InputSubscribe()
    {
        Controls.Player.Kick.started += kickAction;
        Controls.Player.Punch.started += punchAction;
    }

    public void InputUnsubscribe()
    {
        Controls.Player.Kick.started -= kickAction;
        Controls.Player.Punch.started -= punchAction;
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
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
        }
        
    }

    private void OnDestroy()
    {
        //TODO: These don't actually unsubscribe themselves
        Controls.Player.Jump.performed -= _ => airborneJumpTimer = Time.time + airborneJumpDelay;
        Controls.Player.Jump.started -= _ => IsHoldingJumpButton = true;
        Controls.Player.Jump.canceled -= _ => IsHoldingJumpButton = false;

        Controls.Player.Movement.performed -= ctx => MovementX = ctx.ReadValue<Vector2>().x;
        Controls.Player.Movement.canceled -= _ => MovementX = 0;
    }
}
