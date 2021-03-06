using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Playables;

public class InteractableSign : SerializedMonoBehaviour
{
    [SerializeField] private Interactable interactable;
    [SerializeField] private ITweenAnimation[] animationObjs;
    [SerializeField] private GameObject alertObj;
    [SerializeField] private PlayableDirector timeline;

    private void Start()
    {
        interactable.OnInteract += OpenMenu;
        interactable.OnAreaEnter += ShowIndicationArrow;
        interactable.OnAreaExit += HideIndicationArrow;
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
        interactable.OnInteract -= OpenMenu;
        interactable.OnAreaEnter -= ShowIndicationArrow;
        interactable.OnAreaExit -= HideIndicationArrow;
    }
}
