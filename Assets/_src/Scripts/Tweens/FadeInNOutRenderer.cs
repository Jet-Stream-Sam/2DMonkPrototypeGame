using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInNOutRenderer : MonoBehaviour, ITweenAnimation
{
    [SerializeField] private GameObject fadingObject;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 0.5f;

    private LTDescr currentAnimation;

    public bool IsFadingIn { get; private set; }
    public bool IsFadingOut { get; private set; }

    public Action hasFadeInAnimationFinished;
    public Action hasFadeOutAnimationFinished;

    [Button("Fade In")]

    public void OnSelect()
    {
        if(currentAnimation != null)
        {
            if (LeanTween.isTweening(currentAnimation.uniqueId))
            {
                LeanTween.cancel(gameObject, currentAnimation.uniqueId);
            }
        }
        currentAnimation = LeanTween.alpha(fadingObject, 1, fadeInTime).setOnComplete(hasFadeInAnimationFinished).setIgnoreTimeScale(true);
    }
    [Button("Fade Out")]
    public void OnDeselect()
    {
        if (currentAnimation != null)
        {
            if (LeanTween.isTweening(currentAnimation.uniqueId))
            {
                LeanTween.cancel(gameObject, currentAnimation.uniqueId);
            }
        }
        currentAnimation = LeanTween.alpha(fadingObject, 0, fadeOutTime).setOnComplete(hasFadeOutAnimationFinished).setIgnoreTimeScale(true);
    }


}