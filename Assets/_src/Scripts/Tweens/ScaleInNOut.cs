using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ScaleInNOut : MonoBehaviour, ITweenAnimation
{
    [ShowIf("scaleMode", ScaleMode.MultiplyScale)]
    public float scaleRate = 2f;
    [ShowIf("scaleMode", ScaleMode.FixedScale)]
    public Vector3 scaleInValue;
    public float time = 0.2f;
    public ScaleMode scaleMode = ScaleMode.MultiplyScale;
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

    public enum ScaleMode
    {
        MultiplyScale,
        FixedScale
    }

    private void Awake()
    {
        originalScale = transform.localScale;

        if (activationType == ActivationType.Continuous)
        {
            if (scaleMode == ScaleMode.MultiplyScale)
            {
                newScale = originalScale * scaleRate;
            }
            else
            {
                newScale = scaleInValue;
            }
            currentAnimation = LeanTween.scale(gameObject, newScale, time).setIgnoreTimeScale(true).setLoopPingPong().setEase(tweenType);
        }
    }
    public void OnSelect()
    {
        if (activationType == ActivationType.Continuous)
            return;

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
        
        if(scaleMode == ScaleMode.MultiplyScale)
        {
            newScale = originalScale * scaleRate;
        }
        else
        {
            newScale = scaleInValue;
        }
        
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
