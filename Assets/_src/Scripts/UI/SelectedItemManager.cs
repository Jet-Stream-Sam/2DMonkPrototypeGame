using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItemManager : MonoBehaviour
{
    public int selectedItemIndex = -1;
    private List<ITweenAnimation> lastItem;

    private void Awake()
    {
        foreach (var item in GetComponentsInChildren<SelectedItemReciever>())
        {
            item.OnItemSelected += ItemSelected;
        }
    }

    void ItemSelected(int itemIndex, List<ITweenAnimation> items)
    {
        if (itemIndex != selectedItemIndex)
        {
            
            if (lastItem != null)
            {
                foreach (ITweenAnimation item in lastItem)
                {
                    item.OnDeselect();
                }
            }
            if (items != null)
            {
                foreach (ITweenAnimation item in items)
                {
                    item.OnSelect();
                }

            }
            selectedItemIndex = itemIndex;
            lastItem = items;
        }

    }

    private void OnDisable()
    {
        
        if (lastItem != null)
        {
            foreach (ITweenAnimation item in lastItem)
            {
                item.OnDeselect();
            }
        }
        lastItem = null;
        selectedItemIndex = -1;
        
        
    }
}
