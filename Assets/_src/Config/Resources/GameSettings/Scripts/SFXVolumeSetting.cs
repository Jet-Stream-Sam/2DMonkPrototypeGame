using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXVolumeSetting : MonoBehaviour, ISetting
{
    public string SettingName { get; set; } = "SFXVolume";
    [SerializeField] private AudioMixer mixer;

    public void SetChanges(object value)
    {
        if (!(value is float))
        {
            return;
        }

        float volume = (float)value;
        float resultVolume = SettingsUtils.ConvertVolumeToMixer(volume, -40, 0);
        mixer.SetFloat(SettingName, resultVolume);


        int dataIndex = SettingsUtils.ContainsConfigKey(SettingsConfig.configData, SettingName);
        if (dataIndex != -1)
        {
            SettingsConfig.configData[dataIndex].settingValue = volume;
        }
        else
        {
            SettingsConfig.configData.Add(new ConfigData(SettingName, volume));
        }
    }

    

}
