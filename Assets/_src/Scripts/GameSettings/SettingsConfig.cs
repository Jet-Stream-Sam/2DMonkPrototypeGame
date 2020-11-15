
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsConfig : MonoBehaviour
{
    public static List<ConfigData> configData { get; private set; }  = new List<ConfigData>();
    public static List<ISetting> allSettings { get; private set; } = new List<ISetting>();

    private void Awake()
    {
        
        GameObject[] allSettingsInstances = Resources.LoadAll<GameObject>("GameSettings/Prefabs");

        foreach (GameObject instance in allSettingsInstances)
        {
            allSettings.Add(instance.GetComponent<ISetting>());
        }

        if(configData.Count == 0)
        {
            configData = SaveData.LoadAll<ConfigData>("/config/");
        }
        
    }

    private void Start()
    {
        foreach (ISetting setting in allSettings)
        {
            int index = ContainsConfigKey(configData, setting.SettingName);
            if (index != -1)
            {
                
                setting.SetChanges(configData[index].settingValue);
            }
        }
    }
    public void ApplySavedChanges()
    {
        foreach (ConfigData dataComponent in configData)
        {
            ConfigData data = new ConfigData(dataComponent.settingKey, dataComponent.settingValue);
            SaveData.Save(data, "/config/", dataComponent.settingKey);
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
