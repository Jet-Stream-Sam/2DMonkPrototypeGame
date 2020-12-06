using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTween : MonoBehaviour
{
    [SerializeField] private float timeRotating = 0.2f;
    [Range(-360, 360)][SerializeField] private float directionAdd = -360f;
    void OnEnable()
    {
        LeanTween.rotateAround(gameObject, Vector3.forward, directionAdd, timeRotating).setLoopClamp();
    }

}
