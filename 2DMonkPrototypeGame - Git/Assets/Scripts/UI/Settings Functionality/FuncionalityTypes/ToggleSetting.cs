using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleSetting : MonoBehaviour, ISettingsFuncionality
{
    public bool isActivated = false;
    public TextMeshProUGUI textDisplay;
    public ToggleSettingEvent OnValueChanged;


    private void Awake()
    {
        ChangeDisplayText();
    }
    public void SwitchRight()
    {
        isActivated = !isActivated;
        OnValueChanged?.Invoke(isActivated);
        ChangeDisplayText();
    }

    public void SwitchLeft()
    {
        isActivated = !isActivated;
        OnValueChanged?.Invoke(isActivated);
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
