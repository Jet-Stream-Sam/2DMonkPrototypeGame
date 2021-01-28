using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScrollRectPageItem : MonoBehaviour, ISelectHandler
{
    private ControlManager controlManager;
    private InputMaster controls;

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

    private void Start()
    {
        controlManager = ControlManager.Instance;
        controls = controlManager.controls;
        controls.UIExtra.SwitchOptions.performed += OnButtonPressed;
    }

    private void OnEnable()
    {
        if (controls == null)
            return;
        controls.UIExtra.SwitchOptions.performed -= OnButtonPressed;
        controls.UIExtra.SwitchOptions.performed += OnButtonPressed;
    }

    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        float options = context.ReadValue<float>();

        if(options == -1)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject && pageItemFuncionality != null) pageItemFuncionality.SwitchLeft();
        }
        else if(options == 1)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject && pageItemFuncionality != null) pageItemFuncionality.SwitchRight();
        }
    }

    private void OnDisable()
    {
        controls.UIExtra.SwitchOptions.performed -= OnButtonPressed;
    }
    private void OnDestroy()
    {
        controls.UIExtra.SwitchOptions.performed -= OnButtonPressed;
    }

}
