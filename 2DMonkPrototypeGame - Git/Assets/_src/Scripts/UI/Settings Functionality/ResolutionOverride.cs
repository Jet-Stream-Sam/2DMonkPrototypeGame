using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ArrowListSetting))]
public class ResolutionOverride : MonoBehaviour
{
    private ArrowListSetting setting;
    private Resolution[] resolutions;
    void Awake()
    {
        setting = GetComponent<ArrowListSetting>();
        resolutions = Screen.resolutions;

        List<string> options = setting.options;
        options.Clear();

        if(resolutions.Length == 0)
        {
            options.Add("Unsupported");
            return;
        }
        int currentResolution = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            
            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolution = i;
            }
        }

        
        setting.currentOption = currentResolution;
        setting.Refresh();
        
    }

    
}
