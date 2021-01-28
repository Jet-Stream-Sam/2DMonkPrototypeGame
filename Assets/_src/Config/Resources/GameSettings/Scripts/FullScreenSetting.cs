using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FullScreenSetting : MonoBehaviour, ISetting 
{
    public string SettingName { get; set; } = "FullScreen";
    private Resolution[] resolutions;
    public void SetChanges(object value)
    {
        if (!(value is bool))
        {
            return;
        }
        resolutions = Screen.resolutions;

        bool isActivated = (bool)value;
        

        if (resolutions.Length > 0) Screen.fullScreen = isActivated;


        int dataIndex = SettingsUtils.ContainsConfigKey(SettingsConfig.configData, SettingName);
        if (dataIndex != -1)
        {
            SettingsConfig.configData[dataIndex].settingValue = isActivated;
        }
        else
        {
            SettingsConfig.configData.Add(new ConfigData(SettingName, isActivated));
        }
    }

}
