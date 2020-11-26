using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    public List<AttackNotation> availableAttacks = new List<AttackNotation>();
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private PlayerAttackMoveList moveList;
    [SerializeField] private PlayerMainController mainController;
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

    public void ExecuteMove(PlayerAttack attack)
    {
 
        bool isAirbourne = attack.attackNotation.allowedState == "FallingState";
        bool canAttackInTheAir = mainController.canAttackInTheAir;
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
        
    }
    private void UpdateMoveListOnInput(PlayerInputHandler.MovementInputNotation notation)
    {
        currentMovNotation = notation;

        StartCoroutine(UpdateMoveListCo(notation));

    }

    private void UpdateMoveListOnStateChanged(string state)
    {
        CurrentStateOutput = state;
        StartCoroutine(UpdateMoveListCo(currentMovNotation));
    }
    private IEnumerator UpdateMoveListCo(PlayerInputHandler.MovementInputNotation notation)
    {
        
        foreach (PlayerAttack attack in moveList.playerAttackMoveList)
        {
            PlayerInputHandler.MovementInputNotation[] movementNotation = attack.attackNotation.movementNotation;


            if (attack.attackNotation.allowedState == CurrentStateOutput)
            {
                bool freeMovementAttack = movementNotation.Length == 0;
                if (freeMovementAttack)
                {
                    yield return null;
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
                    yield return null;
  
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
