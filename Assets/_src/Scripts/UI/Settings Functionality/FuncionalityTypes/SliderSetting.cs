using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : MonoBehaviour, ISettingsFuncionality
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI rawValueText;
    [RequireInterface(typeof(ISetting))] public UnityEngine.Object settingObject;
    private ISetting settingScript;

    [SerializeField] private float sliderDefaultValue;
    private float sliderStep;

    private void Awake()
    {
        if (settingObject is ISetting setting)
        {
            settingScript = setting;
        }
    }
    private void Start()
    {
        slider.onValueChanged.AddListener(value => SetSliderChanges());
        sliderStep = (slider.maxValue - slider.minValue) / 10;

        
        if (SaveData.SaveAlreadyExists("/config/" + settingScript?.SettingName))
        {
            ConfigData data = SaveData.Load<ConfigData>("/config/" + settingScript?.SettingName);
            slider.value = (float)data.settingValue;
        }
        else
        {
            slider.value = sliderDefaultValue;
        }
            
        ChangeDisplayText(slider.value);
    }
    public void SwitchRight()
    {
        slider.value += sliderStep;
        
    }

    public void SwitchLeft()
    {
        slider.value -= sliderStep;
    }

    void ChangeDisplayText(float value)
    {
        rawValueText.text = value.ToString();
    }

    void SetSliderChanges()
    {
        settingScript.SetChanges(slider.value);
        ChangeDisplayText(slider.value);
    }
}
