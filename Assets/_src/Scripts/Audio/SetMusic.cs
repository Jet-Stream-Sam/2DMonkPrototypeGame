using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusic : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] private bool stopCurrentMusic;
    [HideIf("stopCurrentMusic")]
    [SerializeField] private GameMusic musicToPlay;
    [SerializeField] private bool playOnStart = true;
    private void Start()
    {
        soundManager = SoundManager.Instance;

        if (playOnStart)
        {
            PlayMusic();
        }
            
    }

    public void PlayMusic()
    {
        if (stopCurrentMusic)
        {
            soundManager.StopMusic();
            return;
        }
            
        if (musicToPlay == null)
        {
            Debug.LogWarning("There is no music set to play in this SetMusic script! Did you mean to add one?");
            return;
        }
        soundManager.PlayMusic(musicToPlay.name);
    }
}
