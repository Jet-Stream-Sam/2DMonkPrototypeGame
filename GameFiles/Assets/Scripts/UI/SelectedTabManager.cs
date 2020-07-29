using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTabManager : MonoBehaviour
{
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

        foreach (var tab in tabs)
        {
            tab.OnTabSelected += SlotSelected;
        }
        tabs[0].Select();
    }

    void SlotSelected(int tabIndex, IUIAnimation tab)
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
    private void Update()
    {
        InputHandler();

    }
    private void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int newIndex = selectedTabIndex;
            newIndex += 1;
            if(newIndex >= tabs.Length)
            {
                newIndex = 0;
            }
            tabs[newIndex].Select();
        }
        if (Input.GetKeyDown(KeyCode.Q))  
        {
            int newIndex = selectedTabIndex;
            newIndex -= 1;
            if (newIndex < 0)
            {
                newIndex = tabs.Length - 1;
            }
            tabs[newIndex].Select();
        }
    }
}
