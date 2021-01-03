using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInNOutCanvas : MonoBehaviour
{
    [SerializeField] private Image imageRef;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 0.5f;

    public Action hasFadeInAnimationFinished;
    public Action hasFadeOutAnimationFinished;

    [Button("Fade In")]

    public void FadeIn()
    {
        LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime).setOnComplete(hasFadeInAnimationFinished).setIgnoreTimeScale(true);
        
    }
    [Button("Fade Out")]
    public void FadeOut()
    {
        LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setOnComplete(hasFadeOutAnimationFinished);
    }

    
}
