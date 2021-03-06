using System;
using UnityEngine;
using Sirenix.OdinInspector;
public class InteractableDoor : SerializedMonoBehaviour
{
    [FoldoutGroup("Dependencies")]
    [SerializeField] private Interactable interactable;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private ITweenAnimation[] animationObjs;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private LevelChanger levelChanger;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private UnlockedFeatures unlockedFeatures;

    [SerializeField] private bool isLocked;

    private void Start()
    {
        interactable.OnInteract += EnterDoor;
        interactable.OnAreaEnter += ShowIndicationArrow;
        interactable.OnAreaExit += HideIndicationArrow;
    }

    private void ShowIndicationArrow()
    {
        if (isLocked)
            return;
        foreach(var obj in animationObjs)
        {
            obj.OnSelect();
        }
        
    }
    private void HideIndicationArrow()
    {
        if (isLocked)
            return;
        foreach (var obj in animationObjs)
        {
            obj.OnDeselect();
        }
    }
    private void EnterDoor()
    {
        if (isLocked)
            return;
        unlockedFeatures.InitiateUnlockSequence(() => levelChanger.Change());
    }

    private void OnDestroy()
    {
        interactable.OnInteract -= EnterDoor;
        interactable.OnAreaEnter -= ShowIndicationArrow;
        interactable.OnAreaExit -= HideIndicationArrow;
    }
}
