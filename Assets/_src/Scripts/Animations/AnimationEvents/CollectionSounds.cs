using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Scriptable Objects/Animation Events/Collection Sounds")]
public class CollectionSounds : AnimationEventSO
{
    public GameSound[] gameSounds;

    public enum PlayMode
    {
        PlayAllAtTheSameTime,
        Randomized,
        Sequenced
    }
    public PlayMode playMode;

    [ShowIf("playMode", PlayMode.Sequenced)]
    [SerializeField, ReadOnly] private int sequence = 0;
    public void PlaySound(SoundManager soundManager)
    {
        switch (playMode)
        {
            case PlayMode.PlayAllAtTheSameTime:
                foreach (GameSound sound in gameSounds)
                {
                    soundManager.PlayOneShotSFX(sound.name);
                }
                break;
            case PlayMode.Randomized:
                int selectedSound = Random.Range(0, gameSounds.Length);
                if(gameSounds[selectedSound] != null)
                    soundManager.PlayOneShotSFX(gameSounds[selectedSound].name);
                break;
            case PlayMode.Sequenced:
                soundManager.PlayOneShotSFX(gameSounds[sequence].name);
                sequence++;
                if (sequence == gameSounds.Length)
                    sequence = 0;
                break;

        }
        
    }
}
