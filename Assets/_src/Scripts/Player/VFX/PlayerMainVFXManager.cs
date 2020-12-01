using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainVFXManager : MonoBehaviour
{
    [FoldoutGroup("Dependencies")]
    public AfterImageEffectPool afterImageEffectPool;
    [FoldoutGroup("Dependencies")]
    public PlayerDust playerDustParticles;
}
