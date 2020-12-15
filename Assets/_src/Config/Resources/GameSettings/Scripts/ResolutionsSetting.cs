using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResolutionsSetting : MonoBehaviour, ISetting
{
    public string SettingName { get; set; } = "Resolutions";
    private Resolution[] resolutions;
    public void SetChanges(object value)
    {
        if (!(value is int))
        {
            return;
        }
        resolutions = Screen.resolutions;

        int currentResolutionIndex = (int)value;

        if (currentResolutionIndex < resolutions.Length)
        {
            Resolution resolution = resolutions[currentResolutionIndex];
            Screen.SetResolution(resolution.width,
                resolution.height,
                Screen.fullScreen);
        }

        

        int dataIndex = ContainsConfigKey(SettingsConfig.configData, SettingName);
        if (dataIndex != -1)
        {
            SettingsConfig.configData[dataIndex].settingValue = currentResolutionIndex;
        }
        else
        {
            SettingsConfig.configData.Add(new ConfigData(SettingName, currentResolutionIndex));
        }

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
