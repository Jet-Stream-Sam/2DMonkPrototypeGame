using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AnimationSoundsManager : SerializedMonoBehaviour
{
    private SoundManager soundManager;
    void Start()
    {
        soundManager = SoundManager.Instance;
    }

    public void RecieveSound(CollectionSounds sounds)
    {
        if (sounds.gameSounds.Length == 0)
            return;

        sounds.PlaySound(soundManager);
        
    }
}
