using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleSetting : MonoBehaviour, ISettingsFuncionality
{
    [SerializeField] private bool isActivated = false;
    [SerializeField] private TextMeshProUGUI textDisplay;
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
            isActivated = (bool)data.settingValue;
        }
        ChangeDisplayText();
    }
    public void SwitchRight()
    {
        isActivated = !isActivated;
        settingScript.SetChanges(isActivated);
        ChangeDisplayText();
    }

    public void SwitchLeft()
    {
        isActivated = !isActivated;
        settingScript.SetChanges(isActivated);
        ChangeDisplayText();
    }

    void ChangeDisplayText()
    {
        if(isActivated) { textDisplay.text = "V"; }
        if(!isActivated) { textDisplay.text = "X"; }
    }

    [System.Serializable]
    public class ToggleSettingEvent : UnityEvent<bool>
    {
        
    }
}
