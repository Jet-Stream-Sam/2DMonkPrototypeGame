using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsState : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Animator spriteAnimator;

    public string CurrentState { get; private set; }

    public void ChangeAnimationState(string newState, bool overridesLastState)
    {
        if (!overridesLastState)
        {
            if (CurrentState == newState) return;
        }
        
        CurrentState = newState;
        spriteAnimator.Play(Animator.StringToHash(CurrentState), -1, 0f);
    }

    public float GetCurrentAnimationLength()
    {
        return spriteAnimator.GetCurrentAnimatorStateInfo(0).length;
    }
}
