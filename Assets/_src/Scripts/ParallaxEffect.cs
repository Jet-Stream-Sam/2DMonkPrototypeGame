using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Transform movingTransform;
    [HideInInspector] public bool isParallaxEnabledForThisObject = false;
    [SerializeField, HideInInspector] private float startPos;

    [Required]
    [SerializeField] private Transform mainCamera;

    public float parallaxEffect;

    [Title("Edit Mode")]
    [HideIf("isParallaxEnabledForThisObject"), Button("Enable Parallax for this object")]
    public void ActivateParallax()
    {
        isParallaxEnabledForThisObject = true;
        startPos = movingTransform.position.x;
    }
    [Title("Edit Mode")]
    [ShowIf("isParallaxEnabledForThisObject"), Button("Disable Parallax for this object")]
    public void DeactivateParallax()
    {
        isParallaxEnabledForThisObject = false;
        movingTransform.position = new Vector3(startPos, movingTransform.position.y, movingTransform.position.z);
    }

    void Update()
    {
        if (isParallaxEnabledForThisObject)
        {
            float dist = (mainCamera.position.x - startPos) * parallaxEffect;
            movingTransform.position = new Vector3(startPos + dist, movingTransform.position.y, movingTransform.position.z);
        }
    }

}
