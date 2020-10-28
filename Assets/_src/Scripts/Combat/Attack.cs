using JetBrains.Annotations;
using UnityEngine;


public class Attack : ScriptableObject
{
    public string animationName;
    public int damage;
    public string audioClipName;
    public bool lockVelocity = true;
    public bool lockSideSwitch = true;
}
