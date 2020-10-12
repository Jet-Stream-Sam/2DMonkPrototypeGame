using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuShowPages : MonoBehaviour, ITabFunction
{
    public List<GameObject> pages;
    public ScrollRect scroll;
    public Scrollbar scrollbar;
    public void OnActivate(int index)
    {
        for (int i = 0; i < pages.Count; i++)
        {
            if (i == index)
            {
                pages[i].SetActive(true);

                MenuFirstSelected menuScript = pages[i].GetComponent<MenuFirstSelected>();
                menuScript.ChangeFirstButtonSelected();

                scroll.content = pages[i].GetComponent<RectTransform>();
                continue;
            }
            pages[i].SetActive(false);
             
        }

        if(scroll.GetComponent<ScrollRectAdjust>() != null) scroll.GetComponent<ScrollRectAdjust>().Refresh(scroll.content.GetComponentsInChildren<ScrollRectPageItem>());
        scrollbar.value = 1;
    }

    public void OnDeactivate(int index)
    {

    }

    
}
