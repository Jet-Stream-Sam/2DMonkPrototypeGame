using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffects : MonoBehaviour
{
    [SerializeField] private HitCheck mainHitBox;
    [SerializeField] private Transform VFXTransform;
    [SerializeField] private Transform hitPoint;
    

    private void Start()
    {
        if (mainHitBox == null)
            return;
        mainHitBox.OnSucessfulHit += ApplyEffect;
    }

    private void ApplyEffect(Vector3 pos, IDamageable hitBox)
    {
        if (hitBox == null)
            return;
        GameObject effectPrefab = mainHitBox.HitProperties.visualEffects;

        if (effectPrefab == null)
            return;

        GameObject effectObj = Instantiate(effectPrefab, hitPoint.position, Quaternion.identity, VFXTransform);

        Cinemachine.CinemachineImpulseSource impulseSource =
                effectObj.GetComponent<Cinemachine.CinemachineImpulseSource>();
        impulseSource.GenerateImpulse(transform.up);

    }

    private void OnDestroy()
    {
        if (mainHitBox == null)
            return;
        mainHitBox.OnSucessfulHit -= ApplyEffect;
    }
}
