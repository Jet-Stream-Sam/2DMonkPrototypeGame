using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInNOutRenderer : MonoBehaviour
{
    [SerializeField] private GameObject fadingObject;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 0.5f;

    private LTDescr currentAnimation;

    public Action hasFadeInAnimationFinished;
    public Action hasFadeOutAnimationFinished;

    [Button("Fade In")]

    public void FadeIn()
    {
        if(currentAnimation != null)
        {
            if (LeanTween.isTweening(currentAnimation.uniqueId))
            {
                LeanTween.cancel(currentAnimation.uniqueId);
            }
        }
 
        currentAnimation = LeanTween.alpha(fadingObject, 1, fadeInTime).setOnComplete(hasFadeInAnimationFinished).setIgnoreTimeScale(true);
    }
    [Button("Fade Out")]
    public void FadeOut()
    {
        if (currentAnimation != null)
        {
            if (LeanTween.isTweening(currentAnimation.uniqueId))
            {
                LeanTween.cancel(currentAnimation.uniqueId);
            }
        }
        currentAnimation = LeanTween.alpha(fadingObject, 0, fadeOutTime).setOnComplete(hasFadeOutAnimationFinished).setIgnoreTimeScale(true);
    }

}