using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItemReciever : MonoBehaviour, ISelectHandler
{
    public event System.Action<int, List<ITweenAnimation>> OnItemSelected;
    private List<ITweenAnimation> itemSelectableAnimation;
    public GameObject[] objToAnimate;

    public void OnSelect(BaseEventData data)
    {
        
        if(objToAnimate != null)
        {
            itemSelectableAnimation = new List<ITweenAnimation>();
            
            foreach (GameObject obj in objToAnimate)
            {
                itemSelectableAnimation.Add(obj.GetComponent<ITweenAnimation>());
            }
        }
        OnItemSelected?.Invoke(transform.GetSiblingIndex(), itemSelectableAnimation);
    }
}
