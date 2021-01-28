using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLightingSetting : MonoBehaviour, ISetting
{
    public string SettingName { get; set; } = "ParticleLighting";


    public static int ParticleLighting { get; private set; }
    public static Action<int> OnSettingsChanged;
    public void SetChanges(object value)
    {
        if (!(value is int particleLightingIndex))
        {
            return;
        }

        ParticleLighting = particleLightingIndex;
        int dataIndex = SettingsUtils.ContainsConfigKey(SettingsConfig.configData, SettingName);
        if (dataIndex != -1)
        {
            SettingsConfig.configData[dataIndex].settingValue = particleLightingIndex;
        }
        else
        {
            SettingsConfig.configData.Add(new ConfigData(SettingName, particleLightingIndex));
        }
        OnSettingsChanged?.Invoke(particleLightingIndex);
    }

}
