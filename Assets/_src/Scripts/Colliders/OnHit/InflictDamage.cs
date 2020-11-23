using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflictDamage : MonoBehaviour
{
    [SerializeField] private HitCheck mainHitBox;
    [SerializeField] private ColliderDirection colliderDirection;
    void Start()
    {
        mainHitBox.OnSucessfulHit += ApplyDamage;
    }

    private void ApplyDamage(Vector3 pos, IDamageable hitBox)
    {
        Vector3 direction = colliderDirection.AttackDirection;
        mainHitBox.HitProperties.SetForceDirection(direction);
        hitBox.TakeDamage(mainHitBox.HitProperties.damage, 
            mainHitBox.HitProperties.ForceDirection, mainHitBox.HitProperties.knockbackForce);
    }

    private void OnDestroy()
    {
        mainHitBox.OnSucessfulHit -= ApplyDamage;
    }

}
