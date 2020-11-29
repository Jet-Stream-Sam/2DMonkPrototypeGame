using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float startPos;

    [SerializeField] private Transform mainCamera;

    public float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dist = (mainCamera.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z); 
    }
}
