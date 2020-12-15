using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInflictDamage : MonoBehaviour
{
    [SerializeField] private HitCheck mainHitBox;
    [SerializeField] private ColliderDirection colliderDirection;
    void Start()
    {
        if (mainHitBox == null)
            return;
        mainHitBox.OnSucessfulHit += ApplyDamage;
    }

    private void ApplyDamage(Vector3 pos, IDamageable hitBox)
    {
        if (hitBox == null)
            return;

        if (mainHitBox.HitProperties.hitStunType == HitProperties.HitStunType.Normal)
        {
            hitBox.TakeDamage(mainHitBox.HitProperties.damage);
            return;
        }

        Vector3 direction;
        if(mainHitBox.HitProperties.knockbackMode == HitProperties.KnockbackMode.AssignedFromColliderDirection)
            direction = colliderDirection.AttackDirection;   
        else
            direction = pos - mainHitBox.transform.position;
        mainHitBox.HitProperties.SetForceDirection(direction);
        hitBox.TakeDamage(mainHitBox.HitProperties.damage,
            mainHitBox.HitProperties.ForceDirection, mainHitBox.HitProperties.knockbackForce);
    }

    private void OnDestroy()
    {
        if (mainHitBox == null)
            return;

        mainHitBox.OnSucessfulHit -= ApplyDamage;
    }

}
