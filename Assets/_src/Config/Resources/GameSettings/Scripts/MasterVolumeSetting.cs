using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterVolumeSetting : MonoBehaviour, ISetting
{
    public string SettingName { get; set; } = "MasterVolume";
    [SerializeField] private AudioMixer mixer;

    public void SetChanges(object value)
    {
        if(!(value is float))
        {
            return;
        }

        float volume = (float)value;

        float resultVolume = ConvertVolumeToMixer(volume, -40, 0);
        mixer.SetFloat(SettingName, resultVolume);

        

        int dataIndex = ContainsConfigKey(SettingsConfig.configData, SettingName);
        if (dataIndex != -1)
        {
            SettingsConfig.configData[dataIndex].settingValue = volume;
        }
        else
        {
            SettingsConfig.configData.Add(new ConfigData(SettingName, volume));
        }

        
    }

    private float ConvertVolumeToMixer(float volume, float min, float max)
    {
        if(volume == 0)
        {
            return -80;
        }
        float normalizedVolume = Mathf.InverseLerp(0, 10, volume);
        float resultVolume = Mathf.Lerp(min, max, normalizedVolume);

        return resultVolume;
    }
    public int ContainsConfigKey(List<ConfigData> data, string key)
    {
        int dataIndex = 0;
        foreach (ConfigData dataComponent in data)
        {
            if (dataComponent.settingKey == key)
            {

                return dataIndex;
            }
            dataIndex++;
        }
        return -1;
    }
}
