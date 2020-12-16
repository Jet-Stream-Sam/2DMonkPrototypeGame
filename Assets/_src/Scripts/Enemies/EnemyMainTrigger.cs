using UnityEngine;
using System.Collections;

public class EnemyMainTrigger : MonoBehaviour, IDamageable
{
    public EnemyMainController enemyController;

    public void TakeDamage(int damage)
    {
        enemyController.TakeDamage(damage);
    }
    public void TakeDamage(int damage, Vector2 forceDirection, float knockbackForce)
    {
        enemyController.TakeDamage(damage, forceDirection, knockbackForce);
    }

    public void AnimationSendObject(ScriptableObject obj)
    {
        enemyController.AnimationSendObject(obj);
    }
}
