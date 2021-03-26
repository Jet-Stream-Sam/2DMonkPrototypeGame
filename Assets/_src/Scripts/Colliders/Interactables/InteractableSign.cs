using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Playables;

public class InteractableSign : SerializedMonoBehaviour, IInteractable
{
    [SerializeField] private TriggeredInteraction interaction;
    [SerializeField] private ITweenAnimation[] animationObjs;
    [SerializeField] private GameObject alertObj;
    [SerializeField] private PlayableDirector timeline;

    private void Start()
    {
        interaction.Interact += OpenMenu;
        interaction.AreaEntered += ShowIndicationArrow;
        interaction.AreaExited += HideIndicationArrow;
    }

    private void ShowIndicationArrow()
    {
        foreach(var obj in animationObjs)
        {
            obj.OnSelect();
        }
    }
    private void HideIndicationArrow()
    {
        foreach (var obj in animationObjs)
        {
            obj.OnDeselect();
        }
    }
    private void OpenMenu()
    {
        alertObj.SetActive(false);
        timeline.time = 0;
        timeline.Play();
    }

    private void OnDestroy()
    {
        interaction.Interact -= OpenMenu;
        interaction.AreaEntered -= ShowIndicationArrow;
        interaction.AreaExited -= HideIndicationArrow;
    }
}
