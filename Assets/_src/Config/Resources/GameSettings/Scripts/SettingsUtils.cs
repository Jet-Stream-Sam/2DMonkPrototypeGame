using System.Collections.Generic;
using UnityEngine;

public class SettingsUtils
{
    public static int ContainsConfigKey(List<ConfigData> data, string key)
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

    public static float ConvertVolumeToMixer(float volume, float min, float max)
    {
        if (volume == 0)
        {
            return -80;
        }
        float normalizedVolume = Mathf.InverseLerp(0, 10, volume);
        float resultVolume = Mathf.Lerp(min, max, normalizedVolume);

        return resultVolume;
    }

    public static float ConvertValueFromSlider(float value, float min, float max)
    {

        float normalizedValue = Mathf.InverseLerp(0, 10, value);
        float resultValue = Mathf.Lerp(min, max, normalizedValue);

        return resultValue;
    }
}
