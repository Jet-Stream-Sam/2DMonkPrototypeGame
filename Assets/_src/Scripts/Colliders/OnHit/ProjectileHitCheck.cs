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
        IDamageable hitBox = hitCollider.GetComponent<IDamageable>();

        
        if (hitInstanceException != null)
        {
            
            if (hitBox == hitInstanceException)
            {
                return;
            }
        }

        if (hitBox != null)
        {
            if (hitBox.GetType() == hitTypeException)
            {
                return;
            }
            OnSucessfulHit?.Invoke(hitCollider.transform.position, hitBox);
            checkedHitColliders.Add(hitCollider);
        }

        Tilemap tile = hitCollider.GetComponent<Tilemap>();
        
        if(tile != null)
        {
            OnSucessfulHit?.Invoke(hitCollider.transform.position, null);
            checkedHitColliders.Add(hitCollider);
        }
    }

}
