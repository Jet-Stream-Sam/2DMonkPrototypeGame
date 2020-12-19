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
    private float startPos;

    [Required]
    [SerializeField] private Transform mainCamera;

    public float parallaxEffect;

    [Title("Edit Mode")]
    [HideIf("isParallaxEnabledForThisObject"), Button("Enable Parallax for this object")]
    public void ActivateParallax()
    {
        isParallaxEnabledForThisObject = true;
        startPos = transform.position.x;
    }
    [Title("Edit Mode")]
    [ShowIf("isParallaxEnabledForThisObject"), Button("Disable Parallax for this object")]
    public void DeactivateParallax()
    {
        isParallaxEnabledForThisObject = false;
        transform.position = new Vector3(startPos, transform.position.y, transform.position.z);
    }


    void Start()
    {
        if(isParallaxEnabledForThisObject)
            startPos = transform.position.x;
    }

    void Update()
    {
        if (isParallaxEnabledForThisObject)
        {
            float dist = mainCamera.position.x * parallaxEffect;
            movingTransform.position = new Vector3(startPos + dist, movingTransform.position.y, movingTransform.position.z);
        }
        
    }

}
