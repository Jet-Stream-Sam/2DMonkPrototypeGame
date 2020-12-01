using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainController : MonoBehaviour, IDamageable
{
    [FoldoutGroup("Dependencies")]
    [SerializeField] private EnemyGroan enemyGroan;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private AnimationsState enemyAnimationsScript;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private Collider2D enemyCollider;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private Rigidbody2D enemyRigidBody;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private Transform groundCheck;
    [FoldoutGroup("Dependencies")]
    [SerializeField] public AnimationClip hitAnimationClip;
    [FoldoutGroup("Dependencies")]
    [SerializeField] public AnimationClip deathAnimationClip;

    [TitleGroup("Enemy", Alignment = TitleAlignments.Centered)]
    [TabGroup("Enemy/Tabs", "Movement Settings")]
    [Range(0, 1f)][SerializeField] private float groundedStunnedToIdleEasingRate = 0.6f;
    [TabGroup("Enemy/Tabs", "Movement Settings")]
    [Range(0, 1f)] [SerializeField] private float airborneStunnedToIdleEasingRate = 0.6f;

    [TabGroup("Enemy/Tabs", "Collision Checks")]
    public float groundCheckRadius = 0.25f;
    [TabGroup("Enemy/Tabs", "Collision Checks")]
    public LayerMask groundMask;
    private bool isGrounded;
    private bool hasRecovered = true;
    private bool hasNormalizedMovement = true;
    private bool isStunned => enemyRigidBody.bodyType == RigidbodyType2D.Dynamic;

    [TabGroup("Enemy/Tabs", "Combat")]
    [SerializeField] private int maxHealth;
    [TabGroup("Enemy/Tabs", "Combat")]
    [ReadOnly]
    [SerializeField] private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyAnimationsScript.ChangeAnimationState("wizard_idle");
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundMask);

        
        if (isGrounded && isStunned && hasRecovered)
        {
            bool hasStopped = Mathf.Abs(enemyRigidBody.velocity.x) < 0.01f && enemyRigidBody.velocity.y < 0.01f;
            if (hasStopped)
            {
                hasNormalizedMovement = true;
                enemyRigidBody.bodyType = RigidbodyType2D.Kinematic;
                enemyRigidBody.velocity = Vector2.zero;
            }
            
        }

        
    }

    private void FixedUpdate()
    {

        if (isStunned && isGrounded && !hasNormalizedMovement)
        {
            enemyRigidBody.velocity = Vector2.Lerp(enemyRigidBody.velocity, Vector2.zero, groundedStunnedToIdleEasingRate);
        }
        else if (!isGrounded && !hasRecovered || !isGrounded && !hasNormalizedMovement)
        {
            enemyRigidBody.velocity = Vector2.Lerp(enemyRigidBody.velocity, Vector2.zero, airborneStunnedToIdleEasingRate);
        }
    }
    public void TakeDamage(int damage)
    {
        
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            
            return;
        }
        enemyAnimationsScript.ChangeAnimationState(hitAnimationClip.name);
        StartCoroutine(ComeBackToState("wizard_idle", hitAnimationClip.length));
    }

    public void TakeDamage(int damage, Vector2 forceDirection, float knockbackForce)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= damage;
        enemyRigidBody.bodyType = RigidbodyType2D.Dynamic;
        enemyRigidBody.AddForce(forceDirection * knockbackForce, ForceMode2D.Impulse);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }
        enemyAnimationsScript.ChangeAnimationState(hitAnimationClip.name);
        StartCoroutine(ComeBackToState("wizard_idle", hitAnimationClip.length));
    }

    private void Die()
    {
        enemyCollider.enabled = false;
        enemyRigidBody.Sleep();
        enemyAnimationsScript.ChangeAnimationState(deathAnimationClip.name);
        Destroy(gameObject, deathAnimationClip.length);
    }

    private IEnumerator ComeBackToState(string state, float time)
    {
        hasRecovered = false;
        hasNormalizedMovement = false;
        yield return new WaitForSeconds(time);
        hasRecovered = true;
        enemyAnimationsScript.ChangeAnimationState(state);
    }
}
