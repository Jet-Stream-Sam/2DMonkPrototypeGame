using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleEffect : MonoBehaviour
{
    [SerializeField] private HitCheck mainHitBox;
    [SerializeField] private Transform VFXTransform;

    void Start()
    {
        mainHitBox.OnSucessfulHit += ApplyEffect;
    }

    private void ApplyEffect(Vector3 pos, IDamageable hitBox)
    {
        if (hitBox == null)
            return;

        var particlePrefab = mainHitBox.HitProperties.particleHitEffect;

        if(particlePrefab != null)
        {
            GameObject particleObj = Instantiate(particlePrefab, pos, Quaternion.identity, VFXTransform);

        }
            
    }

    private void OnDestroy()
    {
        mainHitBox.OnSucessfulHit -= ApplyEffect;
    }
}
