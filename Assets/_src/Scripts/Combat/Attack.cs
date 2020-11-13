using System;
using System.Collections.Generic;
using UnityEngine;


public class Attack : ScriptableObject
{
    [Header("Dependencies")]
    public AnimationClip animationClip;
    public GameSound gameSoundAsset;
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

    public HitProperties(HitProperties hitProperties)
    {
        damage = hitProperties.damage;
        knockbackForce = hitProperties.knockbackForce;
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
        Debug.Log(direction);
        ForceDirection = direction;
    }
}
