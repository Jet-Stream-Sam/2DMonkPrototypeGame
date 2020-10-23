using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyGroan : MonoBehaviour
{
    private SoundManager soundManager;
    private void Start() => soundManager = SoundManager.Instance;
    
    
    void Update()
    {
        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        if(kb.jKey.wasPressedThisFrame) soundManager.PlayOneShotSFX("weird_sound");
    }
}
