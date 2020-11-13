
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage);
    void TakeDamage(int damage, Vector2 forceDirection, float knockbackForce);

}
