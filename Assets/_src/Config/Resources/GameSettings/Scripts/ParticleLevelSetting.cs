using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class ParticleLevelSetting : MonoBehaviour, ISetting
{
    public string SettingName { get; set; } = "ParticleLevel";

    public static int ParticleLevel { get; private set; }

    public static Action<int> OnSettingsChanged;
    public void SetChanges(object value)
    {
        if (!(value is int particleLevelIndex))
        {
            return;
        }

        ParticleLevel = particleLevelIndex;
        int dataIndex = SettingsUtils.ContainsConfigKey(SettingsConfig.configData, SettingName);
        if (dataIndex != -1)
        {
            SettingsConfig.configData[dataIndex].settingValue = particleLevelIndex;
        }
        else
        {
            SettingsConfig.configData.Add(new ConfigData(SettingName, particleLevelIndex));
        }
        OnSettingsChanged?.Invoke(particleLevelIndex);
    }
}
