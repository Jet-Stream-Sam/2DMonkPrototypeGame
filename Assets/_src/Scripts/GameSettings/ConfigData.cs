using System;
using System.Diagnostics;

[Serializable]
public class ConfigData
{
    public string settingKey;
    public object settingValue;

    public ConfigData(string key, object value)
    {
        settingKey = key;
        settingValue = value;
    }

    
}
