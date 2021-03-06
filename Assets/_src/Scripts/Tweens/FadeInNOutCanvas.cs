using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInNOutCanvas : MonoBehaviour, ITweenAnimation
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 0.5f;
    [SerializeField] private bool fadeInIgnoreTimeScale = true;
    [SerializeField] private bool fadeOutIgnoreTimeScale;

    private LTDescr currentAnimation;

    public Action hasFadeInAnimationFinished;
    public Action hasFadeOutAnimationFinished;

    [Button("Fade In")]

    public void OnSelect()
    {
        if (currentAnimation != null)
        {
            if (LeanTween.isTweening(currentAnimation.uniqueId))
            {
                LeanTween.cancel(gameObject, currentAnimation.uniqueId);
            }
        }
        if(fadeInIgnoreTimeScale)
            currentAnimation = LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime).setOnComplete(hasFadeInAnimationFinished).setIgnoreTimeScale(true);
        else
            currentAnimation = LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime).setOnComplete(hasFadeInAnimationFinished);
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
        if (fadeOutIgnoreTimeScale)
            currentAnimation = LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setOnComplete(hasFadeOutAnimationFinished).setIgnoreTimeScale(true);
        else
            currentAnimation = LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setOnComplete(hasFadeOutAnimationFinished);
    }

    
}
