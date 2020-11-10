using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ArrowListSetting : MonoBehaviour, ISettingsFuncionality
{
    [SerializeField] private TextMeshProUGUI textDisplay;
    public List<string> options;
    public int currentOption = 0;
    [RequireInterface(typeof(ISetting))] public UnityEngine.Object settingObject;
    private ISetting settingScript;

    private void Awake()
    {
        if (settingObject is ISetting setting)
        {
            settingScript = setting;
        }
    }
    private void Start()
    {
        
        if (SaveData.SaveAlreadyExists("/config/" + settingScript?.SettingName))
        {
            ConfigData data = SaveData.Load<ConfigData>("/config/" + settingScript?.SettingName);
            currentOption = (int)data.settingValue;
        }
        textDisplay.text = options[currentOption];
    }
    public void SwitchRight()
    {
        currentOption += 1;
        if (currentOption >= options.Count) currentOption = 0;

        settingScript.SetChanges(currentOption);
        
        textDisplay.text = options[currentOption];
        
    }

    public void SwitchLeft()
    {
        currentOption -= 1;
        if (currentOption < 0) currentOption = options.Count - 1;

        settingScript.SetChanges(currentOption);
        textDisplay.text = options[currentOption];
    }

    public void Refresh()
    {
        textDisplay.text = options[currentOption];
    }

    [System.Serializable]
    public class ArrowSettingEvent : UnityEvent<int>
    {

    }
}
