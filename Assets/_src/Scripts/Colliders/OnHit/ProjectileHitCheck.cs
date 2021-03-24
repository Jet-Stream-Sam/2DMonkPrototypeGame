using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProjectileHitCheck : HitCheck
{

    [Title("Projectile Hit Properties")]
    [HideLabel] 
    public HitProperties projectileHitProperties;

    private void Start()
    {
        HitProperties = projectileHitProperties;
    }
    public override void OnTriggerEnter2D(Collider2D hitCollider)
    {
        if (HitProperties == null)
            return;
        if (checkedHitColliders.Contains(hitCollider))
            return;

        if (hitCollider.TryGetComponent(out IDamageable hitBox))
        {
            if (hitInstanceException != null)
            {
                if (hitBox == hitInstanceException)
                {
                    return;
                }
            }
            if (hitBox.GetType() == hitTypeException)
            {
                return;
            }
            OnSucessfulHit?.Invoke(hitCollider.transform.position, hitBox);
            checkedHitColliders.Add(hitCollider);
        }

        if(hitCollider.TryGetComponent(out Tilemap tile))
        {
            OnSucessfulHit?.Invoke(hitCollider.transform.position, null);
            checkedHitColliders.Add(hitCollider);
        }
    }

}
