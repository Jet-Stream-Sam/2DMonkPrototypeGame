using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    private ControlManager controlManager;
    private InputMaster controls;
    private float deadzoneMin;
    [SerializeField] private float detectionRadius;
    [SerializeField] private Transform areaTransform;
    [SerializeField] private LayerMask areaMask;
    [SerializeField] private bool debugActivated;

    private Collider2D[] targets = new Collider2D[1];
    private int targetsCount;

    public Action OnInteract;
    public Action OnAreaEnter;
    public Action OnAreaExit;

    private bool isAreaOccupied;

    private void Start()
    {
        controlManager = ControlManager.Instance;
        controls = controlManager.controls;

        controls.Player.Movement.performed += Interact;

        deadzoneMin = InputSystem.settings.defaultDeadzoneMin;
    }

    private void Update()
    {
        targetsCount = Physics2D.OverlapCircleNonAlloc(areaTransform.position, detectionRadius, targets, areaMask);

        if(targetsCount == 0)
        {
            targets = new Collider2D[1];
            if (isAreaOccupied)
                OnAreaExit?.Invoke();
            isAreaOccupied = false;
        }
        else
        {
            if (!isAreaOccupied)
                OnAreaEnter?.Invoke();
            isAreaOccupied = true;
        }
    }
    private void Interact(InputAction.CallbackContext ctx)
    {
        if (targetsCount == 0)
            return;
        float movementY = ctx.ReadValue<Vector2>().y;
        if (movementY > deadzoneMin)
            OnInteract?.Invoke();
    }


    private void OnDrawGizmosSelected()
    {
        if (!debugActivated)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(areaTransform.position, detectionRadius);
    }

    private void OnDestroy()
    {
        controls.Player.Movement.performed -= Interact;
    }
}
