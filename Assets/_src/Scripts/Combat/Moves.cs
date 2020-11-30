using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Moves : SerializedScriptableObject
{
    
    [Title("Dependencies")]
    [PropertyOrder(-3), Required] public AnimationClip animationClip;
    [PropertyOrder(-3)] public GameSound moveSoundEffect;
    [PropertySpace]
    [PropertyOrder(-2)][Button("Debug AnimationClip info", ButtonSizes.Medium)]
    public void GetAnimationClipInfo()
    {
        Debug.Log("");
        Debug.Log($"Animation Name: {animationClip.name}");
        Debug.Log($"Animation Frame Rate: {animationClip.frameRate}");
        Debug.Log($"Animation Frames: {Mathf.RoundToInt(animationClip.length * animationClip.frameRate)}");
        Debug.Log($"Animation Length in Seconds: {animationClip.length}");

    }

    [Title("Move Settings")]
    [PropertyOrder(-1)] public bool lockVelocity = true;
    [PropertyOrder(-1)] public bool lockSideSwitch = true;
    public enum MoveType
    {
        Attack,
        Neutral
    }

    [PropertyOrder(-1)] public MoveType moveType;
    [ShowIf("moveType", MoveType.Attack)]
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
