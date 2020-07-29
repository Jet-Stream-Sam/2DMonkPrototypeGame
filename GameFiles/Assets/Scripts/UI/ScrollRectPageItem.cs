using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectPageItem : MonoBehaviour, ISelectHandler
{
    public System.Action<int> OnPageItemSelected;

    private ISettingsFuncionality pageItemFuncionality;
    public void OnSelect(BaseEventData data)
    {
        OnPageItemSelected?.Invoke(transform.GetSiblingIndex());
        
    }

    
    private void Awake()
    {
        pageItemFuncionality = GetComponent<ISettingsFuncionality>();
    }

    private void Update()
    {
        InputHandler();
        
    }

    private void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Return))
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject && pageItemFuncionality != null) pageItemFuncionality.SwitchRight();
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject && pageItemFuncionality != null) pageItemFuncionality.SwitchLeft();
        }
    }
}
