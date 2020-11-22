using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleEffectAdjust : MonoBehaviour
{
    public Renderer rippleRenderer;
    private const float rippleRadiusOffset = 0.11f;
    private MaterialPropertyBlock propertyBlock;

    public int rippleAmount = 2;
    public float rippleLength = 0.8f;
    public float disappearingRate = 0.5f;
    public float rippleSpeed = 5;

    [ColorUsageAttribute(true,true)][SerializeField] public Color color;
    float rippleAlpha;
    float rippleRadius;
    void Start()
    {
        propertyBlock = new MaterialPropertyBlock();
        rippleAlpha = rippleRenderer.material.GetFloat("_RippleAlpha");

        rippleRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetInt("_RippleAmount", rippleAmount);
        
        rippleRadius = (-rippleAmount / 2) - (rippleAmount * rippleRadiusOffset);
        propertyBlock.SetFloat("_RippleRadius", rippleRadius);
        propertyBlock.SetFloat("_RippleLength", rippleLength);
        propertyBlock.SetColor("_InnerRippleColor", color);
        propertyBlock.SetVector("_RipplePosition", transform.position);

        rippleRenderer.SetPropertyBlock(propertyBlock);

    }

    void Update()
    {
        rippleAlpha -= Time.deltaTime * disappearingRate;
        rippleRadius += Time.deltaTime * rippleSpeed;

        if (rippleAlpha <= 0) Destroy(gameObject, 0.2f);

        rippleRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_RippleRadius", rippleRadius);
        propertyBlock.SetFloat("_RippleAlpha", rippleAlpha);

        rippleRenderer.SetPropertyBlock(propertyBlock);
    }
}
