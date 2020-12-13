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
        Randomized
    }
    public PlayMode playMode;

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

        }
        
    }
}
