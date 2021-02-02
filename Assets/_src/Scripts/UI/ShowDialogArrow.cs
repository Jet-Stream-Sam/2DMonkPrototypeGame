using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDialogArrow : MonoBehaviour
{
    [SerializeField] private MainDialogHandler dialogHandler;
    [SerializeField] private Image arrowImage;

    private void Start()
    {
        dialogHandler.onDialogStopped += ShowArrow;
        dialogHandler.onDialogResume += HideArrow;
    }

    private void ShowArrow()
    {
        arrowImage.enabled = true;
    }

    private void HideArrow()
    {
        arrowImage.enabled = false;
    }
}
