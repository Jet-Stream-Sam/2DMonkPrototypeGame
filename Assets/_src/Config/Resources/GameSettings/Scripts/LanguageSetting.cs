using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Localization.Settings;

public class LanguageSetting : MonoBehaviour, ISetting
{
    public string SettingName { get; set; } = "Language";

    public static int selectedLocale;
    public static Action<int> OnSettingsChanged;

    public void SetChanges(object value)
    {
        
        if (!(value is int languageIndex))
        {
            return;
        }

        selectedLocale = languageIndex;
        Debug.Log(selectedLocale);

        try
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];
        }
        catch (ArgumentOutOfRangeException ex)
        {
            
        }

        int dataIndex = SettingsUtils.ContainsConfigKey(SettingsConfig.configData, SettingName);
        if (dataIndex != -1)
        {
            SettingsConfig.configData[dataIndex].settingValue = languageIndex;
        }
        else
        {
            SettingsConfig.configData.Add(new ConfigData(SettingName, languageIndex));
        }
        OnSettingsChanged?.Invoke(languageIndex);
    }

}
