using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Sounds")]
public class GameSound : ScriptableObject
{
    [Required]
    public AudioClip audioClip;
    [Required]
    public AudioMixerGroup mixer;
    [Range(0, 1)] public float volume = 1;
    [Range(.1f, 3)] public float pitch = 1;
    [Range(0, 1)] public float spatialBlend;
    public bool loop;
    
}
