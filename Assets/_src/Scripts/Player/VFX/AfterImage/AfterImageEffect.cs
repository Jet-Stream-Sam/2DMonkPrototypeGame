using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageEffect : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    [HideInInspector] public AfterImageEffectPool effectPool;

    [SerializeField] private SpriteRenderer currentRenderer;
    [SerializeField] private SpriteRenderer playerRenderer;


    [SerializeField] private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField] private float initialAlpha = 0.8f;
    [SerializeField] private float fadingRate = 0.85f;

    private void OnEnable()
    {
        alpha = initialAlpha;
        currentRenderer.sprite = playerRenderer.sprite;

        transform.position = player.position;
        transform.rotation = player.rotation;
        transform.localScale = playerRenderer.transform.localScale;
        timeActivated = Time.time;
    }

    private void Update()
    {

        
        if(Time.time >= timeActivated + activeTime)
        {
            effectPool.AddToPool(gameObject);
        }

    }

    private void FixedUpdate()
    {
        alpha *= fadingRate;
        currentRenderer.color = new Color(currentRenderer.color.r,
            currentRenderer.color.g,
            currentRenderer.color.b, alpha);
    }
}
