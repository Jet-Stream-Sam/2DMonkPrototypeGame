using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTabReciever : MonoBehaviour
{
    public event System.Action<int, ITweenAnimation> OnTabSelected;
    private ITweenAnimation tabSelectableAnimation;

    private void Awake()
    {
        tabSelectableAnimation = GetComponent<ITweenAnimation>();
    }
    public void Select()
    {
        OnTabSelected?.Invoke(transform.parent.GetSiblingIndex(), tabSelectableAnimation);
        
    }

    
}
