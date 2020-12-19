using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[HideMonoScript]
public class PlayerMainController : MonoBehaviour, IDamageable, IEntityController
{
    public SoundManager SoundManager { get; private set; }
    private ControlManager controlManager;
    public InputMaster Controls { get; private set; }
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
    public SpriteRenderer playerSpriteRenderer;
    [FoldoutGroup("Dependencies")]
    public HitCheck hitBoxCheck;
    [FoldoutGroup("Dependencies")]
    public PlayerInputHandler playerInputHandler;
    [FoldoutGroup("Dependencies")]
    public PlayerMoveList playerMoveList;
    [FoldoutGroup("Dependencies")]
    public MainVFXManager playerMainVFXManager;
    [FoldoutGroup("Dependencies")]
    public Transform playerProjectileTransform;

    [TitleGroup("Player", Alignment = TitleAlignments.Centered)]
    [TabGroup("Player/Tabs", "Movement Settings")]
    public float standingMoveSpeed = 10;
    [TabGroup("Player/Tabs", "Movement Settings")]
    public float crouchingMoveSpeed = 5;
    [TabGroup("Player/Tabs", "Movement Settings")]
    [Range(0.01f, 1)] public float standingEasingRate = 0.6f;
    [TabGroup("Player/Tabs", "Movement Settings")]
    [Range(0.01f, 1)] public float airborneEasingRate = 0.6f;
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
    [TabGroup("Player/Tabs", "Movement Settings")]
    [Range(0, 1f)] public float groundedStunnedToIdleEasingRate = 0.6f;
    [TabGroup("Player/Tabs", "Movement Settings")]
    [Range(0, 1f)] public float airborneStunnedToIdleEasingRate = 0.6f;


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

    [TabGroup("Player/Tabs", "Combat")]
    public int maxHealth;
    [TabGroup("Player/Tabs", "Combat")]
    [ReadOnly]
    public int currentHealth;
    [TabGroup("Player/Tabs", "Combat")]
    [ColorUsage(true, true)]
    public Color hitColor;

    [TabGroup("Player/Tabs", "Debug")]
    [SerializeField] private bool debugActivated = true;
    [TabGroup("Player/Tabs", "Debug")]
    [ShowIf("debugActivated")] [ReadOnly] public bool isGrounded;
    [TabGroup("Player/Tabs", "Debug")]
    [ShowIf("debugActivated")] [ReadOnly] public bool hasRecovered = true;
    [TabGroup("Player/Tabs", "Debug")]
    [ShowIf("debugActivated")] [ReadOnly] public bool hasNormalizedMovement = true;
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
    public Action<ScriptableObject> AnimationEventWasCalled { get; set; }
    public Action hasPerformedJump;
    public Action hasShotAProjectile;
    #endregion

    #region Player Coroutines
    private IEnumerator flashCoroutine;
    #endregion

    #region Animation Event Exclusive Methods
    public void AnimationSendObject(ScriptableObject obj)
    {
        AnimationEventWasCalled?.Invoke(obj);
    }

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

        currentHealth = maxHealth;

        StateMachine = new MainStateMachine();
        
        StateMachine.onStateChanged += state => currentStateOutput = state;
        StateMachine.Init(new PlayerStandingState(this, StateMachine));
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

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();

            return;
        }

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = HitFlash(playerSpriteRenderer, 1f);
        StartCoroutine(flashCoroutine);
    }
    public void TakeDamage(int damage, Vector2 forceDirection, float knockbackForce)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= damage;
        float originalYVelocity = playerRigidBody.velocity.y;
        playerRigidBody.AddForce(forceDirection * knockbackForce, ForceMode2D.Impulse);
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, originalYVelocity);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = HitFlash(playerSpriteRenderer, 3.5f);
        StartCoroutine(flashCoroutine);

        StateMachine.ChangeState(new PlayerHitStunnedState(this, StateMachine));
        

        
    }

    private void Die()
    {
        
    }

    IEnumerator HitFlash(Renderer renderer, float secondsToRecover)
    {
        float lerpRate = 0;
        Color previousMaterialColor = renderer.material.GetColor("_SpriteColor");
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_SpriteColor", hitColor);
        renderer.SetPropertyBlock(propertyBlock);

        yield return null;
        Color currentColor = hitColor;
        while (true)
        {
            if (lerpRate >= 1)
                break;

            lerpRate += Time.deltaTime / secondsToRecover;

            currentColor = Color.Lerp(currentColor, previousMaterialColor, lerpRate);
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_SpriteColor", currentColor);
            renderer.SetPropertyBlock(propertyBlock);

            yield return null;
        }



    }
    
}
