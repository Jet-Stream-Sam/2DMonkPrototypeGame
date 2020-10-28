using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCheck : MonoBehaviour
{
    public int Damage { get; set; }
    private void OnTriggerEnter2D(Collider2D hitCollider)
    {
        IDamageable hitBox = hitCollider.GetComponent<IDamageable>();
        hitBox?.TakeDamage(Damage);
        
    }
}
