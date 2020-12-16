using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageEffect : MonoBehaviour
{
    [HideInInspector] public Transform player;
    
    [HideInInspector] public AfterImageEffectPool effectPool;

    [SerializeField] private SpriteRenderer currentRenderer;
    [HideInInspector] public SpriteRenderer playerRenderer;


    [SerializeField] private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    [SerializeField] private float initialAlpha = 0.8f;
    [SerializeField] private float fadingRate = 0.85f;
    [ColorUsage(true, true)]
    [SerializeField] private Color effectColor;

    private void OnEnable()
    {
        currentRenderer.flipX = playerRenderer.flipX;
        currentRenderer.sprite = playerRenderer.sprite;
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        currentRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_SpriteColor", effectColor);
        currentRenderer.SetPropertyBlock(propertyBlock);
        alpha = initialAlpha;

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
