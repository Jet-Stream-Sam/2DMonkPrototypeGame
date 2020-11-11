using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;


public class Attack : ScriptableObject
{
    public int damage;
    public AnimationClip animationClip;
    public AudioClip audioClip;
    public bool lockVelocity = true;
    public bool lockSideSwitch = true;

    [ContextMenu("Debug AnimationClip info")]
    public void GetClipInfo()
    {
        Debug.Log("");
        Debug.Log($"Animation Name: {animationClip.name}");
        Debug.Log($"Animation Frame Rate: {animationClip.frameRate}");
        Debug.Log($"Animation Frames: {Mathf.RoundToInt(animationClip.length * animationClip.frameRate)}");
        Debug.Log($"Animation Length in Seconds: {animationClip.length}");

    }
    
}
