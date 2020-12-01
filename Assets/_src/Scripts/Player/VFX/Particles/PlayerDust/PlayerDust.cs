using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerDust : SerializedMonoBehaviour
{
    public Dictionary<string, ParticleSystem> dustParticles = new Dictionary<string, ParticleSystem>();

}
