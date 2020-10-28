using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItemReciever : MonoBehaviour, ISelectHandler
{
    public event System.Action<int, List<IUIAnimation>> OnItemSelected;
    private List<IUIAnimation> itemSelectableAnimation;
    public GameObject[] objToAnimate;

    public void OnSelect(BaseEventData data)
    {
        
        if(objToAnimate != null)
        {
            itemSelectableAnimation = new List<IUIAnimation>();
            
            foreach (GameObject obj in objToAnimate)
            {
                itemSelectableAnimation.Add(obj.GetComponent<IUIAnimation>());
            }
        }
        OnItemSelected?.Invoke(transform.GetSiblingIndex(), itemSelectableAnimation);
    }
}
