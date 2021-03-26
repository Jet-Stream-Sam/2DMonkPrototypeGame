using System;
using UnityEngine;
using Sirenix.OdinInspector;
public class InteractableDoor : SerializedMonoBehaviour, IInteractable
{
    [FoldoutGroup("Dependencies")]
    [SerializeField] private TriggeredInteraction interaction;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private ITweenAnimation[] animationObjs;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private LevelChanger levelChanger;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private UnlockedFeatures unlockedFeatures;

    [SerializeField] private bool isLocked;

    private void Start()
    {
        interaction.Interact += EnterDoor;
        interaction.AreaEntered += ShowIndicationArrow;
        interaction.AreaExited += HideIndicationArrow;
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
        interaction.Interact -= EnterDoor;
        interaction.AreaEntered -= ShowIndicationArrow;
        interaction.AreaExited -= HideIndicationArrow;
    }
}
