using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlaySoundEffect : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] private HitCheck mainHitBox;
    void Start()
    {
        soundManager = SoundManager.Instance;
        if (mainHitBox == null)
            return;
        mainHitBox.OnSucessfulHit += ApplyEffect;
    }

    private void ApplyEffect(Vector3 pos, IDamageable hitBox)
    {
        if (hitBox == null)
            return;
        CollectionSounds audioClip = mainHitBox.HitProperties.hitSound;

        if(audioClip != null)
            audioClip.PlaySound(soundManager, pos);
    }

    private void OnDestroy()
    {
        if (mainHitBox == null)
            return;

        mainHitBox.OnSucessfulHit -= ApplyEffect;
    }
}
