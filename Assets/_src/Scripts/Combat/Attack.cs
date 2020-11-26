using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Attack : SerializedScriptableObject
{
    [Header("Dependencies")]
    public AnimationClip animationClip;
    public GameSound attackSoundEffect;
    [Header("Attack Settings")]
    public bool lockVelocity = true;
    public bool lockSideSwitch = true;
    [Header("Hit Properties")]
    public HitProperties hitProperties;


    [ContextMenu("Debug AnimationClip info")]
    public void GetAnimationClipInfo()
    {
        Debug.Log("");
        Debug.Log($"Animation Name: {animationClip.name}");
        Debug.Log($"Animation Frame Rate: {animationClip.frameRate}");
        Debug.Log($"Animation Frames: {Mathf.RoundToInt(animationClip.length * animationClip.frameRate)}");
        Debug.Log($"Animation Length in Seconds: {animationClip.length}");

    }
    
    
}

[Serializable]
public class HitProperties
{
    public int damage;
    public Vector2 ForceDirection { get; private set; }
    public float knockbackForce;
    public float timeStopLength = 0.08f;
    [Range(0, 1)] public float timeStopScale = 0;
    public GameSound hitSound;
    public GameObject impulseSource;
    public GameObject rippleEffectAdjust;
    public GameObject particleHitEffect;

    public HitProperties(HitProperties hitProperties)
    {
        damage = hitProperties.damage;
        knockbackForce = hitProperties.knockbackForce;
        timeStopLength = hitProperties.timeStopLength;
        timeStopScale = hitProperties.timeStopScale;
        hitSound = hitProperties.hitSound;
        impulseSource = hitProperties.impulseSource;
        rippleEffectAdjust = hitProperties.rippleEffectAdjust;
        particleHitEffect = hitProperties.particleHitEffect;
    }
    public HitProperties(int damage, float knockbackForce)
    {
        this.damage = damage;
        this.knockbackForce = knockbackForce;

    }

    public void Reset()
    {
        damage = 0;
        ForceDirection = Vector2.zero;
        knockbackForce = 0;
    }

    public void SetForceDirection(Vector2 positionA, Vector2 positionB)
    {
        Vector2 direction = (positionB - positionA).normalized;
        ForceDirection = direction;
    }

    public void SetForceDirection(Vector2 direction)
    {
        ForceDirection = direction;
    }
}
