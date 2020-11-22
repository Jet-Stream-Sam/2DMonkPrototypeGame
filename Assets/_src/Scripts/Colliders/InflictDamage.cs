using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflictDamage : MonoBehaviour
{
    [SerializeField] private HitCheck mainHitBox;
    void Start()
    {
        mainHitBox.OnSucessfulHit += ApplyDamage;
    }

    private void ApplyDamage(Vector3 pos, IDamageable hitBox)
    {
        mainHitBox.HitProperties.SetForceDirection(transform.position, pos);
        hitBox.TakeDamage(mainHitBox.HitProperties.damage, 
            mainHitBox.HitProperties.ForceDirection, mainHitBox.HitProperties.knockbackForce);
    }

    private void OnDestroy()
    {
        mainHitBox.OnSucessfulHit -= ApplyDamage;
    }

}
