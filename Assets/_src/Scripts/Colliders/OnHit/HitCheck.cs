using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitCheck : SerializedMonoBehaviour
{
    public HitProperties HitProperties { get; set; }
    [FoldoutGroup("Dependencies")]
    [SerializeField] protected Collider2D subjectCollider2D;
    [Title("Detection Exceptions")]
    public IDamageable hitInstanceException;
    public System.Type hitTypeException;

    [ReadOnly]
    [SerializeField] protected List<Collider2D> checkedHitColliders = new List<Collider2D>();
    [HideInInspector] public Action<Vector3, IDamageable> OnSucessfulHit;

    public virtual void OnTriggerEnter2D(Collider2D hitCollider)
    {
        if (HitProperties == null)
            return;
        if (checkedHitColliders.Contains(hitCollider))
            return;

        if (!hitCollider.TryGetComponent(out IDamageable hitBox))
            return;

        if(hitInstanceException != null)
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

    public void ResetProperties()
    {
        checkedHitColliders = new List<Collider2D>();
        HitProperties = null;
    }
}
