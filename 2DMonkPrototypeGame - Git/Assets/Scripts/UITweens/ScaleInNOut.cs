using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleInNOut : MonoBehaviour, IUIAnimation
{
    public float scaleRate = 2f;
    public float time = 0.2f;
    public LeanTweenType tweenType = LeanTweenType.easeOutQuint;
    public ActivationType activationType;

    private Vector3 originalScale;
    private Vector3 newScale;
    public enum ActivationType
    {
        PlayOnce,
        Continuous
    }

    private void Awake()
    {
        originalScale = transform.localScale;
    }
    public void OnSelect()
    {
        if (LeanTween.isTweening(gameObject)) 
        {
            LeanTween.cancel(gameObject);
            transform.localScale = originalScale;
        }

        newScale = transform.localScale * scaleRate;
        LTDescr animation = LeanTween.scale(gameObject, newScale, time).setEase(tweenType);

    }

    public void OnDeselect()
    {
        if (LeanTween.isTweening(gameObject))
        {
            LeanTween.cancel(gameObject);
            transform.localScale = newScale;
        }

        LTDescr animation = LeanTween.scale(gameObject, originalScale, time).setEase(tweenType);
          
    }

}
