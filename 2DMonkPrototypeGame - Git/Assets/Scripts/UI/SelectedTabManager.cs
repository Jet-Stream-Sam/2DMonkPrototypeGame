using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectedTabManager : MonoBehaviour
{
    private GameManager globalManager;
    private InputMaster controls;

    public int selectedTabIndex = -1;


    private IUIAnimation lastTab;
    private ITabFunction tabFunction;
    private SelectedTabReciever[] tabs;

    private void Awake()
    {
        tabFunction = GetComponent<ITabFunction>();
        tabs = GetComponentsInChildren<SelectedTabReciever>();
    }
    private void Start()
    {
        globalManager = GameManager.Instance;
        controls = globalManager.controls;

        controls.UIExtra.SwitchTabs.performed += OnButtonPressed;
        foreach (var tab in tabs)
        {
            tab.OnTabSelected += TabSelected;
        }
        tabs[0].Select();
    }

    void TabSelected(int tabIndex, IUIAnimation tab)
    {
        
        if(tabIndex != selectedTabIndex && lastTab != tab)
        {
            if (lastTab != null) lastTab.OnDeselect();
            if(tab != null) tab.OnSelect();
            selectedTabIndex = tabIndex;
            lastTab = tab;
            
        }
        if (tabFunction != null) tabFunction.OnActivate(tabIndex);
    }


    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        float options = context.ReadValue<float>();

        if (options == -1)
        {
            int newIndex = selectedTabIndex;
            newIndex -= 1;
            if (newIndex < 0)
            {
                newIndex = tabs.Length - 1;
            }
            tabs[newIndex].Select();
        }
        else if (options == 1)
        {
            int newIndex = selectedTabIndex;
            newIndex += 1;
            if (newIndex >= tabs.Length)
            {
                newIndex = 0;
            }
            tabs[newIndex].Select();
        }
        
    }

    private void OnDestroy()
    {
        controls.UIExtra.SwitchTabs.performed -= OnButtonPressed;
    }
}
