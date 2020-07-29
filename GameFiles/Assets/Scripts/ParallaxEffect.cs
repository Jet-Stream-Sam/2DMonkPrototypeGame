using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float startPos;

    private Transform mainCamera;

    public float parallaxEffect;

    void Start()
    {
        mainCamera = transform.root.transform;
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = (mainCamera.position.x * parallaxEffect);
        transform.position = new Vector2(startPos + dist, transform.position.y); 
    }
}
