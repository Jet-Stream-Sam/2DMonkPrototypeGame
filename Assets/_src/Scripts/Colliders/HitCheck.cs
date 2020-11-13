using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCheck : MonoBehaviour
{
    public HitProperties HitProperties { get; set; }
    [SerializeField] private Collider2D subjectCollider2D;
    [SerializeField] private List<Collider2D> checkedHitColliders = new List<Collider2D>();
    private void OnTriggerEnter2D(Collider2D hitCollider)
    {
        if (checkedHitColliders.Contains(hitCollider))
            return;
        IDamageable hitBox = hitCollider.GetComponent<IDamageable>();
        
        HitProperties.SetForceDirection(transform.position, hitCollider.transform.position);
        hitBox?.TakeDamage(HitProperties.damage, HitProperties.ForceDirection, HitProperties.knockbackForce);
        checkedHitColliders.Add(hitCollider);

    }

    private void Update()
    {
        if (!subjectCollider2D.enabled)
            ResetColliders();
    }

    private void ResetColliders()
    {
        checkedHitColliders = new List<Collider2D>();
    }
}
