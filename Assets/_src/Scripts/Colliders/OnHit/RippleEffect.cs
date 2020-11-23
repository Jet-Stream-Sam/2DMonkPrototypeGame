using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    [SerializeField] private HitCheck mainHitBox;
    [SerializeField] private Transform VFXTransform;
    [SerializeField] private GameObject ripplePrefab;
    [SerializeField] private ColliderDirection colliderDirection;
    

    private void Start()
    {
        mainHitBox.OnSucessfulHit += ApplyEffect;
    }

    private void ApplyEffect(Vector3 pos, IDamageable hitBox)
    {
        Transform hitPoint = colliderDirection.endPoint;
        GameObject rippleObj = Instantiate(ripplePrefab, hitPoint.position, Quaternion.identity, VFXTransform);
        
        var comp = rippleObj.GetComponent<RippleEffectAdjust>();
        
        var rippleSettings = mainHitBox.HitProperties.rippleEffectAdjust.GetComponent<RippleEffectAdjust>();
        

        comp.rippleAmount = rippleSettings.rippleAmount;
        comp.rippleLength = rippleSettings.rippleLength;
        comp.disappearingRate = rippleSettings.disappearingRate;
        comp.rippleSpeed = rippleSettings.rippleSpeed;
        comp.color = rippleSettings.color;
        comp.rippleRenderer = rippleObj.GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        mainHitBox.OnSucessfulHit -= ApplyEffect;
    }
}
