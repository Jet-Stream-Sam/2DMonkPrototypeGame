using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    [FoldoutGroup("Dependencies")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private PlayerAttackMoveList moveList;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private PlayerMainController mainController;

    [ReadOnly] public List<AttackNotation> availableAttacks = new List<AttackNotation>();
    [ReadOnly] public List<PlayerAttack> triggeredAttacks = new List<PlayerAttack>();
    
    public string CurrentStateOutput { get; private set; }

    private PlayerInputHandler.MovementInputNotation currentMovNotation;

    private void Start()
    {
        currentMovNotation = PlayerInputHandler.MovementInputNotation.n;
        StartCoroutine(LateStart());
    }

    public void ClearAllAttacks()
    {
        availableAttacks.Clear();
        StopAllCoroutines();
    }

    public IEnumerator ExecuteMove(PlayerAttack attack)
    {
        triggeredAttacks.Add(attack);

        yield return null;

        if(triggeredAttacks.Count == 0)
        {
            yield break;
        }
        if(triggeredAttacks.Count >= 2)
        {
            attack = ChooseMoveWithHighestPriority();
        }

        ClearAllAttacks();
        triggeredAttacks.Clear();

        bool isAirbourne = attack.attackNotation.allowedState == "FallingState";
        bool canAttackInTheAir = mainController.attacksInTheAir >= 0;
        if (isAirbourne && canAttackInTheAir)
        {
            Debug.Log("Combo Performed: " + attack.name);
            mainController.StateMachine.ChangeState(new AirborneAttackState(mainController,
            mainController.StateMachine, attack));
        }
        else if(!isAirbourne)
        {
            Debug.Log("Combo Performed: " + attack.name);
            mainController.StateMachine.ChangeState(new AttackState(mainController,
            mainController.StateMachine, attack));
        }

        
        PlayerAttack ChooseMoveWithHighestPriority()
        {
            int biggestNotation = 0;
            PlayerAttack tempAttack = attack;
            foreach (PlayerAttack tAttack in triggeredAttacks)
            {
                if (tAttack.attackNotation.movementNotation.Length > biggestNotation)
                {
                    tempAttack = tAttack;
                    biggestNotation = tAttack.attackNotation.movementNotation.Length;
                }
            }

            return tempAttack;
        }
    }
    private void UpdateMoveListOnInput(PlayerInputHandler.MovementInputNotation notation)
    {
        currentMovNotation = notation;
        UpdateMoveList(notation);

    }
    private void UpdateMoveListOnStateChanged(string state)
    {
        CurrentStateOutput = state;
        UpdateMoveList(currentMovNotation);
    }
    private void UpdateMoveList(PlayerInputHandler.MovementInputNotation notation)
    {
        
        foreach (PlayerAttack attack in moveList.playerAttackMoveList)
        {
            PlayerInputHandler.MovementInputNotation[] movementNotation = attack.attackNotation.movementNotation;

            if (attack.attackNotation.allowedState == CurrentStateOutput)
            {
                bool freeMovementAttack = movementNotation.Length == 0;
                if (freeMovementAttack)
                {
                    StartCoroutine(inputHandler.InputCheck(attack.attackNotation, attack, this));
                }
                else if(movementNotation[0] == notation)
                {
                    if (inputHandler.HasLateralMovement(movementNotation))
                    {
                        if (!availableAttacks.Contains(attack.attackNotation))
                        {
                            StartCoroutine(inputHandler.InputCheckTimer(attack.attackNotation, attack, true, this));
                            StartCoroutine(inputHandler.InputCheckTimer(attack.attackNotation, attack, false, this));

                        }  
                    }
                    else
                    {
                        if (!availableAttacks.Contains(attack.attackNotation))
                        {
                            StartCoroutine(inputHandler.InputCheckTimer(attack.attackNotation, attack, false, this));
                        }
                    }
  
                }

            } 
        }
    }

    private IEnumerator LateStart()
    {
        yield return null;

        mainController.StateMachine.onStateChanged += UpdateMoveListOnStateChanged;
        inputHandler.onMovementCalled += UpdateMoveListOnInput;

    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        mainController.StateMachine.onStateChanged -= UpdateMoveListOnStateChanged;
        inputHandler.onMovementCalled -= UpdateMoveListOnInput;
    }
}
