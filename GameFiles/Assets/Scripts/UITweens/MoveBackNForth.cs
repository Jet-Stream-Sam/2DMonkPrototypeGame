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
    }
    public void OnSelect()
    {
        if (LeanTween.isTweening(gameObject))
        { 
            LeanTween.cancel(gameObject);
            transform.localPosition = originalPosition;
        }
        newPosition = transform.localPosition + (Vector3)moveDistance;
        LTDescr animation = LeanTween.moveLocal(gameObject, newPosition, time).setEase(tweenType); 
    }

    public void OnDeselect()
    {
        if (LeanTween.isTweening(gameObject))
        {
            LeanTween.cancel(gameObject);
            transform.localPosition = newPosition;
        }

        LTDescr animation = LeanTween.moveLocal(gameObject, originalPosition, time).setEase(tweenType);
    }
 
}
