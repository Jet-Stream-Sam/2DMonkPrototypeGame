using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] private HitCheck mainHitBox;
    void Start()
    {
        soundManager = SoundManager.Instance;
        mainHitBox.OnSucessfulHit += ApplyEffect;
    }

    private void ApplyEffect(Vector3 pos, IDamageable hitBox)
    {

        soundManager.PlayOneShotSFX(mainHitBox.HitProperties.hitSound.name);
    }

    private void OnDestroy()
    {
        mainHitBox.OnSucessfulHit -= ApplyEffect;
    }
}
