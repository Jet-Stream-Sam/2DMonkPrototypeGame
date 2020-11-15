using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainController : MonoBehaviour, IDamageable
{
    [Header("Dependencies")]
    [SerializeField] private EnemyGroan enemyGroan;
    [SerializeField] private AnimationsState enemyAnimationsScript;
    [SerializeField] private Rigidbody2D enemyRigidBody;
    [SerializeField] private Transform groundCheck;

    [Header("Movement Variables")]
    [Range(0, 1f)][SerializeField] private float groundedStunnedToIdleEasingRate = 0.6f;
    [Range(0, 1f)] [SerializeField] private float airborneStunnedToIdleEasingRate = 0.6f;

    [Header("Ground Check")]
    public float groundCheckRadius = 0.25f;
    public LayerMask groundMask;
    private bool isGrounded;
    private bool hasRecovered = true;
    private bool hasNormalizedMovement = true;
    private bool isStunned => enemyRigidBody.bodyType == RigidbodyType2D.Dynamic;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    private int currentHealth;

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

        enemyGroan.OnTrigger();
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            
            return;
        }
        enemyAnimationsScript.ChangeAnimationState("wizard_hit");
        StartCoroutine(ComeBackToState("wizard_idle", 0.15f));
    }

    public void TakeDamage(int damage, Vector2 forceDirection, float knockbackForce)
    {
        currentHealth -= damage;

        enemyGroan.OnTrigger();

        enemyRigidBody.bodyType = RigidbodyType2D.Dynamic;
        enemyRigidBody.AddForce(forceDirection * knockbackForce, ForceMode2D.Impulse);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }
        enemyAnimationsScript.ChangeAnimationState("wizard_hit");
        StartCoroutine(ComeBackToState("wizard_idle", 0.15f));
    }

    private void Die()
    {
        enemyGroan.OnTrigger();
        enemyRigidBody.Sleep();
        enemyAnimationsScript.ChangeAnimationState("wizard_death");
        Destroy(gameObject, 0.45f);
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
