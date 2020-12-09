using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitCheck : SerializedMonoBehaviour
{
    public HitProperties HitProperties { get; set; }
    [SerializeField] private Collider2D subjectCollider2D;
    [SerializeField] private List<Collider2D> checkedHitColliders = new List<Collider2D>();
    [SerializeField] private IDamageable hitException;

    [HideInInspector] public Action<Vector3, IDamageable> OnSucessfulHit;

    private void OnTriggerEnter2D(Collider2D hitCollider)
    {
        if (HitProperties == null)
            return;
        if (checkedHitColliders.Contains(hitCollider))
            return;
        IDamageable hitBox = hitCollider.GetComponent<IDamageable>();
        
        if(hitException != null)
        {
            if (hitBox == hitException)
            {
                Debug.Log(hitBox + " equals " + hitException);
                return;
            }
        }
        
        if(hitBox != null)
        {
            OnSucessfulHit?.Invoke(hitCollider.transform.position, hitBox);
            checkedHitColliders.Add(hitCollider);
        }
    }

    public void ResetProperties()
    {
        checkedHitColliders = new List<Collider2D>();
        HitProperties = null;
    }
}
