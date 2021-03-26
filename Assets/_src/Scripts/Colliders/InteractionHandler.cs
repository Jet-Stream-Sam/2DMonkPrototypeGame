using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHandler : MonoBehaviour
{
    private ControlManager controlManager;
    private InputMaster controls;
    private float deadzoneMin;
    [SerializeField] private PlayerMainController playerMainController;
    [SerializeField] private Transform actorTransform;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private bool debugActivated;

    private Collider2D[] targets = new Collider2D[1];
    private TriggeredInteraction selectedInteractable;
    private int targetsCount;
    private bool isAreaOccupied;

    private void Start()
    {
        controlManager = ControlManager.Instance;
        controls = controlManager.controls;

        controls.Player.Movement.performed += Interact;

        deadzoneMin = InputSystem.settings.defaultDeadzoneMin;
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (selectedInteractable == null)
            return;
        float movementY = ctx.ReadValue<Vector2>().y;
        if (movementY > deadzoneMin)
            selectedInteractable.OnInteract();
    }
    private void Update()
    {
        if (!playerMainController.isGrounded)
        {
            AreaExit();
            return;
        }
        targetsCount = Physics2D.OverlapCircleNonAlloc(actorTransform.position, detectionRadius, targets, interactableMask);

        if (targetsCount == 0)
        { 
            if (isAreaOccupied)
            {
                AreaExit();       
            }
                
            
        }
        else
        {
            if (!isAreaOccupied)
            {
                AreaEnter();
            }
                
            
        }
    }
    
    private void AreaEnter()
    {
        selectedInteractable = targets[0].GetComponent<TriggeredInteraction>();
        if (selectedInteractable == null)
            return;
        isAreaOccupied = true;
        selectedInteractable.OnAreaEnter(); 
    }
    private void AreaExit()
    {
        targets = new Collider2D[1];
        isAreaOccupied = false;
        if (selectedInteractable == null)
            return;
        selectedInteractable.OnAreaExit();
        selectedInteractable = null;
        
    }

    private void OnDrawGizmosSelected()
    {
        if (!debugActivated)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(actorTransform.position, detectionRadius);
    }

    private void OnDestroy()
    {
        controls.Player.Movement.performed -= Interact;
    }
}
