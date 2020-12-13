using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FlipSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sRenderer;
    [SerializeField] private Transform transformToFlip;
    [ReadOnly] public float horizontalDirection;
    private bool wasFlipped;
    public void Flip(float direction)
    {
        horizontalDirection = direction;

        bool isFlipped = false;
        switch (direction)
        {
            case -1:
                isFlipped = false;
                break;
            case 1:
                isFlipped = true;
                break;
        }  
        if (isFlipped != wasFlipped)
        {
            transformToFlip.localPosition =
                new Vector2(-transformToFlip.localPosition.x, transformToFlip.localPosition.y);
            sRenderer.flipX = isFlipped;
        }

        wasFlipped = isFlipped;
    }
}
