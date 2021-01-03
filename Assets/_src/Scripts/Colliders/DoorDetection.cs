using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorDetection : MonoBehaviour
{
    private ControlManager controlManager;
    private InputMaster controls;
    private float deadzoneMin;

    [SerializeField] private Transform doorTransform;
    [SerializeField] private FadeInNOutRenderer fadingRenderer;
    [SerializeField] private LevelChanger levelChanger;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask doorMask;
    [SerializeField] private bool isLocked;
    [SerializeField] private bool debugActivated;

    private Collider2D[] targets = new Collider2D[1];
    private int targetsCount;

    private bool isFadingIn;
    private bool isFadingOut;

    private void Start()
    {
        controlManager = ControlManager.Instance;
        controls = controlManager.controls;

        controls.Player.Movement.performed += Interact;

        deadzoneMin = InputSystem.settings.defaultDeadzoneMin;
    }
    private void Update()
    {
        if (isLocked)
            return;

        targetsCount = Physics2D.OverlapCircleNonAlloc(doorTransform.position, detectionRadius, targets, doorMask);
        if(targetsCount == 0)
        {
            targets = new Collider2D[1];

            if (!isFadingOut)
            {
                fadingRenderer.FadeOut();
                isFadingOut = true;
                isFadingIn = false;
            }
            
        }
        else
        {
            if (!isFadingIn)
            {
                fadingRenderer.FadeIn();
                isFadingIn = true;
                isFadingOut = false;
            }
            
        }
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (isLocked)
            return;
        if (targetsCount == 0)
            return;

        float movementY = ctx.ReadValue<Vector2>().y;
        if (movementY > deadzoneMin)
            levelChanger.ChangeLevel();

    }
    private void OnDrawGizmosSelected()
    {
        if (!debugActivated)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(doorTransform.position, detectionRadius);
    }

    private void OnDestroy()
    {
        controls.Player.Movement.performed -= Interact;
    }
}
