using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : SerializedMonoBehaviour
{
    private ControlManager controlManager;
    private InputMaster controls;

    [FoldoutGroup("Dependencies")]
    [SerializeField] private PlayerMainController controllerScript;
    [TitleGroup("Input")]
    [SerializeField] private float pressedButtonTimeWindow = 1f;
    [ReadOnly] [SerializeField] private float pressedButtonTimer;

    [ReadOnly] [SerializeField] private float holdingButtonTime;
    private float holdingButtonTimeLimit = 5;

    private Vector2 inputAxis;
    private float deadzoneMin;
    [ReadOnly] [SerializeField] private List<int> buttonsCurrentlyPressed = new List<int>();

    #region Input Events
    [HideInInspector] public Action<MovementInputNotation> onMovementCalled;
    [HideInInspector] public Action<ButtonInputNotation> onTimedButtonCalled;
    [HideInInspector] public Action<ButtonInputNotation> onRawButtonCalled;
    private Action<InputAction.CallbackContext> inputAxisAction;
    private Action<InputAction.CallbackContext> inputAxisCancel;

    private Action<InputAction.CallbackContext> inputWestButtonAction;
    private Action<InputAction.CallbackContext> inputNorthButtonAction;
    private Action<InputAction.CallbackContext> inputSouthButtonAction;
    private Action<InputAction.CallbackContext> inputEastButtonAction;

    private Action<InputAction.CallbackContext> inputWestButtonCancel;
    private Action<InputAction.CallbackContext> inputNorthButtonCancel;
    private Action<InputAction.CallbackContext> inputSouthButtonCancel;
    private Action<InputAction.CallbackContext> inputEastButtonCancel;

    #endregion
    
    [Flags]
    public enum MovementInputNotation
    {
        n = 0,
        u = 1,
        d = 2,
        f = 4,
        b = 8,
 
        uf = u | f,
        df = d | f,
        db = d | b,
        ub = u | b
    }

    [Flags]
    public enum OppositeMovementInputNotation
    {
        n = 0,
        u = 1,
        d = 2,
        b = 4,
        f = 8,

        uf = u | f,
        df = d | f,
        db = d | b,
        ub = u | b
    }

    [Flags]
    public enum ButtonInputNotation
    {
        none = 0,
        one = 1,
        two = 2,
        three = 4,
        four = 8,
        
        onePlusTwo = one | two,
        twoPlusThree = two | three,
        threePlusFour = three | four,
        onePlusFour = one | four,
        twoPlusFour = two | four,
        onePlusThree = one | three,
        onePlusTwoPlusThree = one | two | three,
        twoPlusThreePlusFour = two | three | four,
        threePlusFourPlusOne = three | four | one,
        onePlusTwoPlusFour = one | two | four,
        allButtons = one | two | three | four,
    }

    [OdinSerialize] [ReadOnly] public MovementInputNotation MovementInput { get; private set; }

    [OdinSerialize] [ReadOnly] public ButtonInputNotation RawButtonInput { get; private set; }

    [OdinSerialize] [ReadOnly] public ButtonInputNotation PressedButtonInput { get; private set; }

    private void Start()
    {
        controlManager = ControlManager.Instance;
        controls = controlManager.controls;

        deadzoneMin = InputSystem.settings.defaultDeadzoneMin;

        #region Input Handling
        inputAxisAction += OnMovementPerformed;
        inputAxisCancel += OnMovementCanceled;

        inputWestButtonAction += (ctx) => OnButtonPressed(ctx, (int)ButtonInputNotation.one);
        inputNorthButtonAction += (ctx) => OnButtonPressed(ctx, (int)ButtonInputNotation.two);
        inputSouthButtonAction += (ctx) => OnButtonPressed(ctx, (int)ButtonInputNotation.three);
        inputEastButtonAction += (ctx) => OnButtonPressed(ctx, (int)ButtonInputNotation.four);

        inputWestButtonCancel += (ctx) => OnButtonReleased(ctx, (int)ButtonInputNotation.one);
        inputNorthButtonCancel += (ctx) => OnButtonReleased(ctx, (int)ButtonInputNotation.two);
        inputSouthButtonCancel += (ctx) => OnButtonReleased(ctx, (int)ButtonInputNotation.three);
        inputEastButtonCancel += (ctx) => OnButtonReleased(ctx, (int)ButtonInputNotation.four);

        controls.Player.Movement.performed += inputAxisAction;
        controls.Player.Movement.canceled += inputAxisCancel;

        controls.Player.Punch.performed += inputWestButtonAction;
        controls.Player.Kick.performed += inputNorthButtonAction;
        controls.Player.Jump.performed += inputSouthButtonAction;
        controls.Player.Energy.performed += inputEastButtonAction;

        controls.Player.Punch.canceled += inputWestButtonCancel;
        controls.Player.Kick.canceled += inputNorthButtonCancel;
        controls.Player.Jump.canceled += inputSouthButtonCancel;
        controls.Player.Energy.canceled += inputEastButtonCancel;

        #endregion

    }

    public MovementInputNotation ReverseInput(MovementInputNotation notation)
    {
        string n = notation.ToString();
        OppositeMovementInputNotation oppositeNotation =
            (OppositeMovementInputNotation)Enum.Parse(typeof(OppositeMovementInputNotation), n);

        return (MovementInputNotation)oppositeNotation;
    }

    public MovementInputNotation[] ReverseInputAll(MovementInputNotation[] notations)
    {
        MovementInputNotation[] newMovNotation = new MovementInputNotation[notations.Length];

        for (int i = 0; i < newMovNotation.Length; i++)
        {
            newMovNotation[i] = ReverseInput(notations[i]);
        }

        return newMovNotation;
    }

    public bool HasLateralMovement(MovementInputNotation notation)
    {
        if (notation.HasFlag(MovementInputNotation.f) ||
                notation.HasFlag(MovementInputNotation.b))
        {
            return true;
        }

        return false;
    }
    public bool HasLateralMovement(MovementInputNotation[] notations)
    {
        foreach (MovementInputNotation notation in notations)
        {
            if(notation.HasFlag(MovementInputNotation.f) ||
                notation.HasFlag(MovementInputNotation.b))
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator InputCheck(MoveNotation notation, PlayerMoves move, PlayerMoveHandler mainHandler)
    {
        if (!mainHandler.availableMoves.Contains(notation))
        {
            mainHandler.availableMoves.Add(notation);
            while (true)
            {
                if (PressedButtonInput == notation.buttonNotation)
                {
                    StartCoroutine(mainHandler.ExecuteMove(move));
                    
                    break;
                }
                if (notation.allowedState != mainHandler.CurrentStateOutput)
                {
                    mainHandler.availableMoves.Remove(notation);
                    break;
                }

                yield return null;
            }
   
        }

    }
    public IEnumerator InputCheckTimer(MoveNotation notation, PlayerMoves move, bool isReversed, PlayerMoveHandler mainHandler)
    {
        MovementInputNotation[] sequences = notation.movementNotation;

        if (isReversed)
        {
            sequences = ReverseInputAll(sequences);
            
            
        }

        int i = 0;
        float maxTimeBtwInputs = move.moveNotation.allowedTimeBetweenInputs;
        bool hasExecutedMove = false;

        mainHandler.availableMoves.Add(notation);

        while (maxTimeBtwInputs > 0)
        {
            maxTimeBtwInputs -= Time.deltaTime;

            bool firstInput = i == 0;
            bool lastInput = i == sequences.Length;
            if (lastInput)
            {
                if (PressedButtonInput == notation.buttonNotation)
                {
                    StartCoroutine(mainHandler.ExecuteMove(move));
                    hasExecutedMove = true;
                    break;
                }
                else if (PressedButtonInput > 0)
                {
                    break;
                }

                if (MovementInput != sequences[i - 1] && MovementInput > 0)
                {
                    break;
                }

            }

            else if (MovementInput == sequences[i])
            {
                i++;
                maxTimeBtwInputs = move.moveNotation.allowedTimeBetweenInputs;
            }
            else if (!firstInput)
            {
                if (MovementInput != sequences[i] && MovementInput != sequences[i - 1] && MovementInput > 0)
                {
                    break;
                }
            }

            yield return null;
        }
        if (!hasExecutedMove)
        {
            mainHandler.availableMoves.Remove(notation);
        }
    }
    private void OnMovementPerformed(InputAction.CallbackContext ctx)
    {
        inputAxis = ctx.ReadValue<Vector2>();

        bool movementDetected = Mathf.Abs(inputAxis.x) > deadzoneMin ||
           Mathf.Abs(inputAxis.y) > deadzoneMin;
        if (movementDetected)
        {
            int xAxis = InputConvertMovement(inputAxis.x, deadzoneMin, (int)MovementInputNotation.b, (int)MovementInputNotation.f);
            int yAxis = InputConvertMovement(inputAxis.y, deadzoneMin, (int)MovementInputNotation.d, (int)MovementInputNotation.u);

            MovementInput = (MovementInputNotation)xAxis + yAxis;

            onMovementCalled?.Invoke(MovementInput);
        }

        int InputConvertMovement(float axis, float deadzone, int negEnumValue, int posEnumValue)
        {
            if (axis > 0)
            {
                return posEnumValue;
            }
            else if (axis < 0)
            {
                return negEnumValue;
            }
            else
            {
                return 0;
            }

        }
    }

    private void OnMovementCanceled(InputAction.CallbackContext ctx)
    {
        MovementInput = 0;
        onMovementCalled?.Invoke(MovementInput);
    }

    private void OnButtonPressed(InputAction.CallbackContext ctx, int buttonIndex)
    {
        if (buttonsCurrentlyPressed.Contains(buttonIndex))
            return;

        buttonsCurrentlyPressed.Add(buttonIndex);

        int rawInput = 0;

        foreach (int button in buttonsCurrentlyPressed)
        {
            rawInput += button;
        }

        RawButtonInput = (ButtonInputNotation)rawInput;
        PressedButtonInput = (ButtonInputNotation)rawInput;
        StartCoroutine(HoldingButton(RawButtonInput));

        onRawButtonCalled?.Invoke(RawButtonInput);
        
        if (pressedButtonTimer == 0)
        {
            pressedButtonTimer = pressedButtonTimeWindow;
            StartCoroutine(TimedReleaseButtons());
        }
        else
        {
            pressedButtonTimer = pressedButtonTimeWindow;
        }
        

    }

    private void OnButtonReleased(InputAction.CallbackContext ctx, int buttonIndex)
    {
        if (buttonsCurrentlyPressed.Contains(buttonIndex))
        {
            buttonsCurrentlyPressed.Remove(buttonIndex);
        }

        if(buttonsCurrentlyPressed.Count == 0)
        {
            RawButtonInput = 0;
            PressedButtonInput = 0;

        }
        else
        {
            int rawInput = 0;

            foreach (int button in buttonsCurrentlyPressed)
            {
                rawInput += button;
            }

            RawButtonInput = (ButtonInputNotation)rawInput;
            PressedButtonInput = (ButtonInputNotation)rawInput;

            if(pressedButtonTimer == 0)
            {
                pressedButtonTimer = pressedButtonTimeWindow;
                StartCoroutine(TimedReleaseButtons());
            }
            else
            {
                pressedButtonTimer = pressedButtonTimeWindow;
            }
            
        }

        onTimedButtonCalled?.Invoke(PressedButtonInput);
        onRawButtonCalled?.Invoke(RawButtonInput);

    }
    
    private IEnumerator TimedReleaseButtons()
    {
        while(pressedButtonTimer > 0)
        {
            pressedButtonTimer -= Time.deltaTime;
            yield return null;
        }
        pressedButtonTimer = 0;
        PressedButtonInput = 0;
        onTimedButtonCalled?.Invoke(PressedButtonInput);

    }

    private IEnumerator HoldingButton(ButtonInputNotation button)
    {
        while(RawButtonInput == button && holdingButtonTime < holdingButtonTimeLimit)
        {
            holdingButtonTime += Time.deltaTime;
            yield return null;
        }
        holdingButtonTime = 0;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();

        #region Input Handling
        inputAxisAction -= OnMovementPerformed;
        inputAxisCancel -= OnMovementCanceled;

        controls.Player.Movement.performed -= inputAxisAction;
        controls.Player.Movement.canceled -= inputAxisCancel;

        controls.Player.Punch.performed -= inputWestButtonAction;
        controls.Player.Kick.performed -= inputNorthButtonAction;
        controls.Player.Jump.performed -= inputSouthButtonAction;
        controls.Player.Energy.performed -= inputEastButtonAction;

        controls.Player.Punch.canceled -= inputWestButtonCancel;
        controls.Player.Kick.canceled -= inputNorthButtonCancel;
        controls.Player.Jump.canceled -= inputSouthButtonCancel;
        controls.Player.Energy.canceled -= inputEastButtonCancel;
        #endregion
    }
}
