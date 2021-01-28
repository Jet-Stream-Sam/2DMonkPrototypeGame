using System;
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

    private LTDescr currentAnimation;
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
        if (!gameObject.activeInHierarchy)
            return;

        if (currentAnimation != null)
        {
            if (LeanTween.isTweening(currentAnimation.uniqueId))
            {
                LeanTween.cancel(currentAnimation.uniqueId);
                transform.localScale = originalScale;
            }
        }
        
        newScale = originalScale * scaleRate;
        StartCoroutine(SkipFrame(() => { currentAnimation = LeanTween.scale(gameObject, newScale, time).setEase(tweenType).setIgnoreTimeScale(true); }));

    }

    public void OnDeselect()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (currentAnimation != null)
        {
            if (LeanTween.isTweening(currentAnimation.uniqueId))
            {
                LeanTween.cancel(currentAnimation.uniqueId);
                transform.localScale = newScale;
            }
        }
        StartCoroutine(SkipFrame(() => { currentAnimation = LeanTween.scale(gameObject, originalScale, time).setEase(tweenType).setIgnoreTimeScale(true); }));
        
          
    }

    private IEnumerator SkipFrame(Action func)
    {
        yield return null;

        func?.Invoke();
    }
}
