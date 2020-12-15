using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Moves : SerializedScriptableObject
{
    
    [Title("Dependencies")]
    [PropertyOrder(-5), Required] public AnimationClip animationClip;
    [PropertyOrder(-5)] public CollectionSounds moveSoundEffect;
    [PropertySpace]
    [PropertyOrder(-4)][Button("Debug AnimationClip info", ButtonSizes.Medium)]
    public void GetAnimationClipInfo()
    {
        Debug.Log("");
        Debug.Log($"Animation Name: {animationClip.name}");
        Debug.Log($"Animation Frame Rate: {animationClip.frameRate}");
        Debug.Log($"Animation Frames: {Mathf.RoundToInt(animationClip.length * animationClip.frameRate)}");
        Debug.Log($"Animation Length in Seconds: {animationClip.length}");

    }

    [Title("Move Settings")]
    [PropertyOrder(-3)] public bool lockVelocity = true;
    [PropertyOrder(-3)] public bool lockSideSwitch = true;
    public enum MoveType
    {
        Attack,
        Neutral,
        Projectile
    }

    [PropertyOrder(-3)] public MoveType moveType;

    [Title("Projectile Properties")]
    [ShowIf("moveType", MoveType.Projectile)]
    [InfoBox("Projectile prefabs are now assigned in the animation clip's events.")]
    [PropertyOrder(-1)] public string projectiles = "are assigned in the animation clips!";
    [HideInInspector] public bool attackAndProjectile;
    [HideIf("@attackAndProjectile || moveType != MoveType.Projectile")]
    [PropertyOrder(-1)] [Button("Only Shoot Projectile", ButtonSizes.Medium)]
    [GUIColor(0.7f, 1f, 0.2f)]
    void ChangeButton()
    {
        attackAndProjectile = true;
    }
    [ShowIf("@attackAndProjectile && moveType == MoveType.Projectile")]
    [PropertyOrder(-1)] [Button("Attack and Shoot Projectile", ButtonSizes.Medium)]
    [GUIColor(0.7f, 0.2f, 1f)]
    void ChangeButton2()
    {
        attackAndProjectile = false;
    }
    
    
    [Title("Hit Properties")]
    [ShowIf("@moveType == MoveType.Attack || moveType == MoveType.Projectile && attackAndProjectile")]
    [HideLabel]
    public HitProperties hitProperties;

  
}
[Serializable]
public class HitProperties
{
    public enum KnockbackMode
    {
        AssignedFromColliderDirection,
        Manual
    }
    public enum HitStunType
    {
        Normal,
        Special
    }

    
    [TabGroup("Gameplay")]
    public int damage;
    [TabGroup("Gameplay")]
    public HitStunType hitStunType;
    [TabGroup("Gameplay")]
    public Vector2 ForceDirection { get; private set; }
    [TabGroup("Gameplay")]
    [ShowIf("hitStunType", HitStunType.Special)] public float knockbackForce;
    [TabGroup("Gameplay")]
    [ShowIf("hitStunType", HitStunType.Special)] public KnockbackMode knockbackMode = KnockbackMode.AssignedFromColliderDirection;
    [TabGroup("Visuals")]
    public float timeStopLength = 0.08f;
    [TabGroup("Visuals")]
    [Range(0, 1)] public float timeStopScale = 0;
    [TabGroup("SFX")]
    public CollectionSounds hitSound;
    [TabGroup("Visuals")]
    [AssetsOnly]
    public GameObject impulseSource;
    [TabGroup("Visuals")]
    [AssetsOnly]
    public GameObject rippleEffectAdjust;
    [TabGroup("Visuals")]
    [AssetsOnly]
    public GameObject particleHitEffect;

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
