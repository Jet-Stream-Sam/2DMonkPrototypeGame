using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class SkippableHandler : MonoBehaviour
{
    private ControlManager controlManager;
    private InputMaster controls;

    public Action OnAnimationSkipped;

    private void Start()
    {
        controlManager = ControlManager.Instance;
        controls = controlManager.controls;

        controls.Dialog.Advance.performed += SkipAnimation;
    }

    private void SkipAnimation(InputAction.CallbackContext obj)
    {
        OnAnimationSkipped?.Invoke();
    }

}
