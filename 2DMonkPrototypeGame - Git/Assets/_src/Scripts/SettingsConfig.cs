
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsConfig : MonoBehaviour
{
    public AudioMixer mixer;
    private Resolution[] resolutions;
    private void Awake()
    {
        resolutions = Screen.resolutions;
    }
    // VOLUME //
    public void SetMasterVolume(float volume)
    {
        float resultVolume = ConvertVolumeToMixer(volume, -80, 0);
        mixer.SetFloat("MasterVolume", resultVolume);
    }
    public void SetSFXVolume(float volume)
    {
        float resultVolume = ConvertVolumeToMixer(volume, -80, 0);
        mixer.SetFloat("SFXVolume", resultVolume);
    }
    public void SetMusicVolume(float volume)
    {
        float resultVolume = ConvertVolumeToMixer(volume, -80, 0);
        mixer.SetFloat("MusicVolume", resultVolume);
    }

    private float ConvertVolumeToMixer(float volume, float min, float max)
    {
        float normalizedVolume = Mathf.InverseLerp(0, 10, volume);
        float resultVolume = Mathf.Lerp(min, max, normalizedVolume);

        return resultVolume;
    }

    // SCREEN //

    public void SetFullScreen(bool isActivated)
    {
        if(resolutions.Length > 0) Screen.fullScreen = isActivated;
    }

    public void SetResolution(int currentResolutionIndex)
    {
        if (resolutions.Length > 0)
        {
            Resolution resolution = resolutions[currentResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
            
    }
    
}
