using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTabReciever : MonoBehaviour
{
    public event System.Action<int, IUIAnimation> OnTabSelected;
    private IUIAnimation tabSelectableAnimation;

    private void Awake()
    {
        tabSelectableAnimation = GetComponent<IUIAnimation>();
    }
    public void Select()
    {
        OnTabSelected?.Invoke(transform.parent.GetSiblingIndex(), tabSelectableAnimation);
        
    }

    
}
