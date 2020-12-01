using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[HideMonoScript]
public class PlayerMainController : MonoBehaviour
{
    public SoundManager SoundManager { get; private set; }
    private ControlManager controlManager;
    public InputMaster Controls { get; private set; }
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isHittingHead;
    [HideInInspector] public int attacksInTheAir = 0;
    [HideInInspector] public bool isReversed = false;
    public float MovementX { get; private set; }
    public float MovementY { get; private set; }
    public bool IsHoldingJumpButton { get; private set; }
    [FoldoutGroup("Dependencies", expanded: false)]
    public Transform groundCheck;
    [FoldoutGroup("Dependencies")]
    public Transform ceilingCheck;
    [FoldoutGroup("Dependencies")]
    public Rigidbody2D playerRigidBody;
    [FoldoutGroup("Dependencies")]
    public CapsuleCollider2D playerMainCollider;
    [FoldoutGroup("Dependencies")]
    public AnimationsState playerAnimationsScript;
    [FoldoutGroup("Dependencies")]
    public Transform playerSpriteTransform;
    [FoldoutGroup("Dependencies")]
    public HitCheck hitBoxCheck;
    [FoldoutGroup("Dependencies")]
    public PlayerInputHandler playerInputHandler;
    [FoldoutGroup("Dependencies")]
    public PlayerMoveList playerMoveList;
    [FoldoutGroup("Dependencies")]
    public PlayerMainVFXManager playerMainVFXManager;

    [TitleGroup("Player", Alignment = TitleAlignments.Centered)]
    [TabGroup("Player/Tabs", "Movement Settings")]
    public float standingMoveSpeed = 10;
    [TabGroup("Player/Tabs", "Movement Settings")]
    public float crouchingMoveSpeed = 5;
    [TabGroup("Player/Tabs", "Movement Settings")]
    [Range(0.01f, 1)] public float easingRate = 0.6f;
    [TabGroup("Player/Tabs", "Movement Settings")]
    public float jumpSpeed = 2;
    [TabGroup("Player/Tabs", "Movement Settings")]
    public float jumpHeight = 5;
    [TabGroup("Player/Tabs", "Movement Settings")]
    public float airborneJumpDelay = 0.2f;
    [TabGroup("Player/Tabs", "Movement Settings")]
    public float groundedJumpDelay = 0.2f;
    [TabGroup("Player/Tabs", "Movement Settings")]
    [HideInInspector] public float airborneJumpTimer;
    [TabGroup("Player/Tabs", "Movement Settings")]
    [HideInInspector] public float groundedJumpTimer;
    [TabGroup("Player/Tabs", "Movement Settings")]
    public float fallMultiplier = 1.5f;
    

    [TabGroup("Player/Tabs", "Collision Checks")]
    [TabGroup("Player/Tabs/Collision Checks/SubTabGroup", "Ground Check")]
    public float groundCheckRadius = 1f;
    [TabGroup("Player/Tabs/Collision Checks/SubTabGroup", "Ground Check")]
    public LayerMask groundMask;

    [TabGroup("Player/Tabs", "Collision Checks")]
    [TabGroup("Player/Tabs/Collision Checks/SubTabGroup", "Ceiling Check")]
    public float ceilingCheckRadius = 1f;
    [TabGroup("Player/Tabs/Collision Checks/SubTabGroup", "Ceiling Check")]
    public LayerMask ceilingMask;

    [TabGroup("Player/Tabs", "Debug")]
    [SerializeField] private bool debugActivated = true;
    public MainStateMachine StateMachine { get; private set; }
    [TabGroup("Player/Tabs", "Debug")]
    [ShowIf("debugActivated")]
    [ReadOnly]
    public string currentStateOutput;

    #region Input Events
    public Action<InputAction.CallbackContext> kickAction;
    public Action<InputAction.CallbackContext> punchAction;
    #endregion
    #region Player Events
    public Action hasPerformedJump;
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


        #endregion

        StateMachine = new MainStateMachine();
        
        StateMachine.onStateChanged += state => currentStateOutput = state;
        StateMachine.Init(new StandingState(this, StateMachine));
    }

    private void Update()
    {
        StateMachine.CurrentState.HandleUpdate();
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
