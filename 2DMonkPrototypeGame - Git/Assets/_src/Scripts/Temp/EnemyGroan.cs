using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyGroan : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] private string groanSound;
    private void Start() => soundManager = SoundManager.Instance;
    

    public void OnTrigger()
    {
        soundManager.PlayOneShotSFX(groanSound);
    }
}
