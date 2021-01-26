using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusic : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] private GameMusic musicToPlay;
    [SerializeField] private bool playOnStart = true;
    private void Start()
    {
        soundManager = SoundManager.Instance;

        if(playOnStart)
            soundManager.PlayMusic(musicToPlay.name);
    }

    public void PlayMusic()
    {
        soundManager.PlayMusic(musicToPlay.name);
    }
}
