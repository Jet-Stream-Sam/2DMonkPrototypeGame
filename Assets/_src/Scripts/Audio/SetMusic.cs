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

        if (playOnStart)
        {
            if (musicToPlay == null)
            {
                Debug.LogWarning("There is no music set to play in this SetMusic script! Did you mean to add one?");
                return;
            }
            soundManager.PlayMusic(musicToPlay.name);
        }
            
    }

    public void PlayMusic()
    {
        if (musicToPlay == null)
        {
            Debug.LogWarning("There is no music set to play in this SetMusic script! Did you mean to add one?");
            return;
        }
        soundManager.PlayMusic(musicToPlay.name);
    }
}
