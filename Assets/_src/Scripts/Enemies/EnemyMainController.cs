using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainController : MonoBehaviour, IDamageable, IEntityController
{
    [FoldoutGroup("Dependencies")]
    public EnemyGroan enemyGroan;
    [FoldoutGroup("Dependencies")]
    public AnimationsState enemyAnimationsScript;
    [FoldoutGroup("Dependencies")]
    public Collider2D enemyCollider;
    [FoldoutGroup("Dependencies")]
    public Rigidbody2D enemyRigidBody;
    [FoldoutGroup("Dependencies")]
    public Transform enemySpriteTransform;
    [FoldoutGroup("Dependencies")]
    public SpriteRenderer enemySpriteRenderer;
    [FoldoutGroup("Dependencies")]
    public Transform groundCheck;
    [FoldoutGroup("Dependencies")]
    public Transform groundDetectionLeft;
    [FoldoutGroup("Dependencies")]
    public Transform groundDetectionRight;
    [FoldoutGroup("Dependencies")]
    public Transform wallDetectionLeft;
    [FoldoutGroup("Dependencies")]
    public Transform wallDetectionRight;
    [FoldoutGroup("Dependencies")]
    public AnimationClip enemyIdle;
    [FoldoutGroup("Dependencies")]
    public AnimationClip hitAnimationClip;
    [FoldoutGroup("Dependencies")]
    public AnimationClip deathAnimationClip;
    [FoldoutGroup("Dependencies")]
    public EnemyAIBrain AIBrain;
    [FoldoutGroup("Dependencies")]
    public Transform enemyProjectileTransform;
    [FoldoutGroup("Dependencies")]
    public HitCheck hitBoxCheck;

    [TitleGroup("Enemy", Alignment = TitleAlignments.Centered)]
    [TabGroup("Enemy/Tabs", "Movement Settings")]
    [Range(0, 1f)] public float groundedStunnedToIdleEasingRate = 0.6f;
    [TabGroup("Enemy/Tabs", "Movement Settings")]
    [Range(0, 1f)] public float airborneStunnedToIdleEasingRate = 0.6f;
    [TabGroup("Enemy/Tabs", "Movement Settings")]
    [SerializeField] public float enemySpeed;

    [TabGroup("Enemy/Tabs", "Collision Checks")]
    [TabGroup("Enemy/Tabs/Collision Checks/SubTabGroup", "Ground Check")]
    public float groundCheckRadius = 0.25f;
    [TabGroup("Enemy/Tabs", "Collision Checks")]
    [TabGroup("Enemy/Tabs/Collision Checks/SubTabGroup", "Ground Detection")]
    public float groundDetectionLRadius = 0.25f;
    [TabGroup("Enemy/Tabs/Collision Checks/SubTabGroup", "Ground Detection")]
    public float groundDetectionRRadius = 0.25f;
    [TabGroup("Enemy/Tabs", "Collision Checks")]
    [TabGroup("Enemy/Tabs/Collision Checks/SubTabGroup", "Wall Detection")]
    public float wallDetectionLRadius = 0.25f;
    [TabGroup("Enemy/Tabs/Collision Checks/SubTabGroup", "Wall Detection")]
    public float wallDetectionRRadius = 0.25f;

    [TabGroup("Enemy/Tabs", "Collision Checks")]
    public LayerMask groundMask;

    [TabGroup("Enemy/Tabs", "Combat")]
    public int maxHealth;
    [TabGroup("Enemy/Tabs", "Combat")]
    [ReadOnly]
    public int currentHealth;

    [TabGroup("Enemy/Tabs", "Debug")]
    [SerializeField] private bool debugActivated = true;
    public MainStateMachine StateMachine { get; private set; }
    [TabGroup("Enemy/Tabs", "Debug")]
    [ShowIf("debugActivated")] [ReadOnly] public string currentStateOutput;
    [TabGroup("Enemy/Tabs", "Debug")]
    [ShowIf("debugActivated")] [ReadOnly] public bool isGrounded;
    [TabGroup("Enemy/Tabs", "Debug")]
    [ShowIf("debugActivated")] [ReadOnly] public bool hasRecovered = true;
    [TabGroup("Enemy/Tabs", "Debug")]
    [ShowIf("debugActivated")] [ReadOnly] public bool hasNormalizedMovement = true;

    #region Enemy Events
    public Action hasShotAProjectile;
    #endregion

    private void Start()
    {
        currentHealth = maxHealth;

        StateMachine = new MainStateMachine();

        StateMachine.onStateChanged += state => currentStateOutput = state;
        StateMachine.Init(new EnemyStandingState(this, StateMachine));

    }

    private void Update()
    {
        StateMachine.CurrentState.HandleUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.HandleFixedUpdate(); 
    }
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            
            return;
        }

        StateMachine.ChangeState(new EnemyHitStunnedState(this, StateMachine));
  
    }

    public void TakeDamage(int damage, Vector2 forceDirection, float knockbackForce)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= damage;
        enemyRigidBody.AddForce(forceDirection * knockbackForce, ForceMode2D.Impulse);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }
        StateMachine.ChangeState(new EnemyHitStunnedState(this, StateMachine));
    }

    private void Die()
    {
        StateMachine.ChangeState(new EnemyDeathState(this, StateMachine));
    }

    private void OnDrawGizmos()
    {
        if (debugActivated)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.DrawWireSphere(groundDetectionLeft.position, groundDetectionLRadius);
            Gizmos.DrawWireSphere(groundDetectionRight.position, groundDetectionRRadius);
            Gizmos.DrawWireSphere(wallDetectionLeft.position, wallDetectionLRadius);
            Gizmos.DrawWireSphere(wallDetectionRight.position, wallDetectionRRadius);
        }

    }

    #region Animation Event Exclusive Methods
    public void ShootProjectile()
    {
        Debug.Log("BOOM HEADSHOT!!!");
        hasShotAProjectile?.Invoke();
    }

    #endregion
}
