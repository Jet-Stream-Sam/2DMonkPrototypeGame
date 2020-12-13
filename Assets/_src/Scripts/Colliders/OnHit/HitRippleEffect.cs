using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRippleEffect : MonoBehaviour
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
        GameObject ripplePrefab = mainHitBox.HitProperties.rippleEffectAdjust;

        if (ripplePrefab == null)
            return;

        GameObject rippleObj = Instantiate(ripplePrefab, hitPoint.position, Quaternion.identity, VFXTransform);

    }

    private void OnDestroy()
    {
        if (mainHitBox == null)
            return;
        mainHitBox.OnSucessfulHit -= ApplyEffect;
    }
}
