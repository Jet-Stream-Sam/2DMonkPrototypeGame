using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Attack : SerializedScriptableObject
{
    
    [Title("Dependencies")]
    [PropertyOrder(-2), Required] public AnimationClip animationClip;
    [PropertyOrder(-2)] public GameSound attackSoundEffect;
    [PropertySpace]
    [PropertyOrder(-1)][Button("Debug AnimationClip info", ButtonSizes.Medium)]
    public void GetAnimationClipInfo()
    {
        Debug.Log("");
        Debug.Log($"Animation Name: {animationClip.name}");
        Debug.Log($"Animation Frame Rate: {animationClip.frameRate}");
        Debug.Log($"Animation Frames: {Mathf.RoundToInt(animationClip.length * animationClip.frameRate)}");
        Debug.Log($"Animation Length in Seconds: {animationClip.length}");

    }

    [Title("Attack Settings")]
    public bool lockVelocity = true;
    public bool lockSideSwitch = true;
    [Title("Hit Properties")]
    [HideLabel]
    public HitProperties hitProperties;
  
}
[Serializable]
public class HitProperties
{
    [TabGroup("Gameplay")]
    public int damage;
    [TabGroup("Gameplay")]
    public Vector2 ForceDirection { get; private set; }
    [TabGroup("Gameplay")]
    public float knockbackForce;
    [TabGroup("Visuals")]
    public float timeStopLength = 0.08f;
    [TabGroup("Visuals")]
    [Range(0, 1)] public float timeStopScale = 0;
    [TabGroup("SFX")]
    public GameSound hitSound;
    [TabGroup("Visuals")]
    [AssetsOnly]
    public GameObject impulseSource;
    [TabGroup("Visuals")]
    [AssetsOnly]
    public GameObject rippleEffectAdjust;
    [TabGroup("Visuals")]
    [AssetsOnly]
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
