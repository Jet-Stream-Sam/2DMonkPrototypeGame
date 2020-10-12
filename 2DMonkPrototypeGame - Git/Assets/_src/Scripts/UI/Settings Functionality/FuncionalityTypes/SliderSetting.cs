using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : MonoBehaviour, ISettingsFuncionality
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI rawValueText;

    [SerializeField] private float sliderDefaultValue;
    private float sliderStep;

    private void Awake()
    {
        sliderStep = (slider.maxValue - slider.minValue) / 10;
        slider.value = sliderDefaultValue;
        ChangeDisplayText(slider.value);
    }
    public void SwitchRight()
    {
        slider.value += sliderStep;
        ChangeDisplayText(slider.value);
        
    }

    public void SwitchLeft()
    {
        slider.value -= sliderStep;
        ChangeDisplayText(slider.value);
    }

    void ChangeDisplayText(float value)
    {
        rawValueText.text = value.ToString();
    }
}
