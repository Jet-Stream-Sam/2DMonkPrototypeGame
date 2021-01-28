using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackNForth : MonoBehaviour, IUIAnimation
{
    public Vector2 moveDistance = new Vector2(30, 30);
    public float time = 0.2f;

    public LeanTweenType tweenType = LeanTweenType.easeOutQuint;
    public ActivationType activationType;

    private LTDescr currentAnimation;


    private Vector3 originalPosition;
    private Vector3 newPosition;

    public enum ActivationType
    {
        PlayOnce,
        Continuous
    }

    private void Awake()
    {
        originalPosition = transform.localPosition;
        if(activationType == ActivationType.Continuous)
        {
            newPosition = transform.localPosition + (Vector3)moveDistance;
            currentAnimation = LeanTween.moveLocal(gameObject, newPosition, time).setIgnoreTimeScale(true).setLoopPingPong().setEase(tweenType);
        }
    }
    public void OnSelect()
    {
        if (activationType == ActivationType.Continuous)
            return;


        LeanTween.cancel(gameObject);
        transform.localPosition = originalPosition;

        newPosition = originalPosition + (Vector3)moveDistance;
        StartCoroutine(SkipFrame(() => { currentAnimation = LeanTween.moveLocal(gameObject, newPosition, time).setEase(tweenType).setIgnoreTimeScale(true); }));
    }

    public void OnDeselect()
    {
        if (activationType == ActivationType.Continuous)
            return;
        if (!gameObject.activeInHierarchy)
            return;

        LeanTween.cancel(gameObject);
        transform.localPosition = newPosition;

        StartCoroutine(SkipFrame(() => { currentAnimation = LeanTween.moveLocal(gameObject, originalPosition, time).setEase(tweenType).setIgnoreTimeScale(true); }));
        
    }

    private IEnumerator SkipFrame(Action func)
    {
        yield return null;

        func?.Invoke();
    }

}
