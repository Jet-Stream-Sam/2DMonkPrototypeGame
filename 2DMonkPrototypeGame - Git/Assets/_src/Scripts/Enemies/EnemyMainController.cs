using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainController : MonoBehaviour, IDamageable
{
    [Header("Dependencies")]
    [SerializeField] private EnemyGroan enemyGroan;
    [SerializeField] private AnimationsState enemyAnimationsScript;
    [Header("Health")]
    [SerializeField] private int maxHealth;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyAnimationsScript.ChangeAnimationState("wizard_idle");
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

    private void Die()
    {
        enemyAnimationsScript.ChangeAnimationState("wizard_death");
        Destroy(gameObject, 0.45f);
    }

    private IEnumerator ComeBackToState(string state, float time)
    {
        yield return new WaitForSeconds(time);
        enemyAnimationsScript.ChangeAnimationState(state);
    }
}
