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
        if(currentAnimation != null)
        {
            if (LeanTween.isTweening(currentAnimation.uniqueId))
            {
                LeanTween.cancel(currentAnimation.uniqueId);
                transform.localScale = originalScale;
            }
        }
        
        newScale = transform.localScale * scaleRate;
        currentAnimation = LeanTween.scale(gameObject, newScale, time).setEase(tweenType).setIgnoreTimeScale(true);

    }

    public void OnDeselect()
    {

        if (currentAnimation != null)
        {
            if (LeanTween.isTweening(currentAnimation.uniqueId))
            {
                LeanTween.cancel(currentAnimation.uniqueId);
                transform.localScale = newScale;
            }
        }

        currentAnimation = LeanTween.scale(gameObject, originalScale, time).setEase(tweenType).setIgnoreTimeScale(true);
          
    }

}
