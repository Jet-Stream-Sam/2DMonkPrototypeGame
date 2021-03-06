using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private Transform musicTransform;
    [SerializeField] private Transform soundsTransform;
    [SerializeField] private AudioSource currentMusic;
    [SerializeField] private GameSound[] gameSounds;
    [SerializeField] private GameMusic[] gameMusic;
    private GameObject emptyGameObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        gameMusic = Resources.LoadAll<GameMusic>("GameMusic");
        gameSounds = Resources.LoadAll<GameSound>("GameSounds");
    }

    private void Start()
    {
        emptyGameObject = new GameObject();
    }
    public void PlayMusic(string name)
    {
        var song = Array.Find(gameMusic, m => m.name == name);

        GameMusicSourceWrap(currentMusic, song);
        currentMusic.Play();

    }
    public void StopMusic()
    {
        currentMusic.Stop();
    }
    public void PlaySFX(string name, Vector3 pos)
    {
        var sound = Array.Find(gameSounds, s => s.name == name);
        var soundObj = Instantiate(emptyGameObject, pos, Quaternion.identity, soundsTransform);
        soundObj.transform.parent = soundsTransform;
        soundObj.name = sound.name;
        
        var source = soundObj.AddComponent<AudioSource>();
        GameSoundSourceWrap(source, sound);
        source.Play();
        if(!source.loop) Destroy(soundObj, sound.audioClip.length);
    }

    public void PlayOneShotSFX(string name, Vector3 pos)
    {
        var sound = Array.Find(gameSounds, s => s.name == name);
        var soundObj = Instantiate(emptyGameObject, pos, Quaternion.identity, soundsTransform);
        
        soundObj.name = sound.name;

        var source = soundObj.AddComponent<AudioSource>();

        GameSoundSourceWrap(source, sound);
        
        source.PlayOneShot(sound.audioClip);
        if (!source.loop) Destroy(soundObj, sound.audioClip.length);

    }

    private void GameSoundSourceWrap(AudioSource source, GameSound sound)
    {
        source.clip = sound.audioClip;
        source.outputAudioMixerGroup = sound.mixer;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.spatialBlend = sound.spatialBlend;
        source.loop = sound.loop;
    }

    private void GameMusicSourceWrap(AudioSource source, GameMusic music)
    {
        source.clip = music.audioClip;
        source.outputAudioMixerGroup = music.mixer;
        source.volume = music.volume;
        source.pitch = music.pitch;
        source.loop = music.loop;
    
    }
}
