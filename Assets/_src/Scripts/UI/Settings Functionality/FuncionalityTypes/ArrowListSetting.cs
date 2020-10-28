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
    public ArrowSettingEvent OnValueChanged;
    private void Awake()
    {
        textDisplay.text = options[currentOption];

    }
    public void SwitchRight()
    {
        currentOption += 1;
        if (currentOption >= options.Count) currentOption = 0;

        OnValueChanged?.Invoke(currentOption);
        textDisplay.text = options[currentOption];
        
    }

    public void SwitchLeft()
    {
        currentOption -= 1;
        if (currentOption < 0) currentOption = options.Count - 1;

        OnValueChanged?.Invoke(currentOption);
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
