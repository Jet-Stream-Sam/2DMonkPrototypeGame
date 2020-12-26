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
        GameMusic song = Array.Find(gameMusic, m => m.name == name);

        currentMusic.clip = song.audioClip;
        currentMusic.outputAudioMixerGroup = song.mixer;
        currentMusic.volume = song.volume;
        currentMusic.pitch = song.pitch;
        currentMusic.loop = song.loop;
        currentMusic.Play();

    }
    public void PlaySFX(string name, Vector3 pos)
    {
        GameSound sound = Array.Find(gameSounds, s => s.name == name);
        GameObject soundObj = Instantiate(emptyGameObject, pos, Quaternion.identity, soundsTransform);
        soundObj.transform.parent = soundsTransform;
        soundObj.name = sound.name;
        
        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = sound.audioClip;
        source.outputAudioMixerGroup = sound.mixer;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.spatialBlend = sound.spatialBlend;
        source.loop = sound.loop;
        source.Play();
        if(!source.loop) Destroy(soundObj, sound.audioClip.length);


    }

    public void PlayOneShotSFX(string name, Vector3 pos)
    {
        GameSound sound = Array.Find(gameSounds, s => s.name == name);
        GameObject soundObj = Instantiate(emptyGameObject, pos, Quaternion.identity, soundsTransform);
        
        soundObj.name = sound.name;

        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = sound.audioClip;
        source.outputAudioMixerGroup = sound.mixer;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.spatialBlend = sound.spatialBlend;
        source.loop = sound.loop;
        
        source.PlayOneShot(sound.audioClip);
        if (!source.loop) Destroy(soundObj, sound.audioClip.length);

    }

}
