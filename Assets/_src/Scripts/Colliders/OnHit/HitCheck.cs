using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitCheck : MonoBehaviour
{
    public HitProperties HitProperties { get; set; }
    [SerializeField] private Collider2D subjectCollider2D;
    [SerializeField] private List<Collider2D> checkedHitColliders = new List<Collider2D>();
    

    public Action<Vector3, IDamageable> OnSucessfulHit;

    private void OnTriggerEnter2D(Collider2D hitCollider)
    {
        if (checkedHitColliders.Contains(hitCollider))
            return;
        IDamageable hitBox = hitCollider.GetComponent<IDamageable>();
 
        if(hitBox != null)
        {
            OnSucessfulHit?.Invoke(hitCollider.transform.position, hitBox);
            checkedHitColliders.Add(hitCollider);
  
        }
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
