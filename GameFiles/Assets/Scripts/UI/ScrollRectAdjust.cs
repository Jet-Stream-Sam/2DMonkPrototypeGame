using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectAdjust : MonoBehaviour
{
    public Scrollbar itemScrollbar;
    public List<ScrollRectPageItem> pageItems;

    public void Refresh(ScrollRectPageItem[] pageItems)
    {
        if(this.pageItems.Count > 0)
        {
            foreach (ScrollRectPageItem item in this.pageItems)
            {
                item.OnPageItemSelected -= Scroll;
            }
            this.pageItems.Clear();
        }
        
        foreach (ScrollRectPageItem item in pageItems)
        {
            this.pageItems.Add(item);
            item.OnPageItemSelected += Scroll;
        }



    }
    public void Scroll(int pageIndex)
    {
        int itemTotal = pageItems.Count;
        itemScrollbar.value = 1f - pageIndex / (float)itemTotal;
        if (itemTotal - 1 == pageIndex) itemScrollbar.value = 0;
    }

    
}
