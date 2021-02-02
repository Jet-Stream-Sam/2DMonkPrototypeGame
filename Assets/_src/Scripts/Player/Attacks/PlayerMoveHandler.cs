using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveHandler : MonoBehaviour
{
    [FoldoutGroup("Dependencies")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private PlayerMoveList moveList;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private PlayerMainController mainController;

    [ReadOnly] public List<MoveNotation> availableMoves = new List<MoveNotation>();
    [ReadOnly] public List<PlayerMoves> triggeredMoves = new List<PlayerMoves>();
    
    public string CurrentStateOutput { get; private set; }

    private PlayerInputHandler.MovementInputNotation currentMovNotation;

    private void Start()
    {
        currentMovNotation = PlayerInputHandler.MovementInputNotation.n;
        StartCoroutine(LateStart());
    }

    public void ClearAllMoves()
    {
        availableMoves.Clear();
        StopAllCoroutines();
    }

    public IEnumerator ExecuteMove(PlayerMoves move)
    {
        if(move.powerMeterUsage > 0)
        {
            if (!CanExecutePowerMove(move.powerMeterUsage))
            {
                availableMoves.Clear();
                yield break;
            }
                
        }
        if (move.vitalityMeterUsage > 0)
        {
            if (!CanExecuteVitalityMove(move.vitalityMeterUsage))
            {
                availableMoves.Clear();
                yield break;
            }
                
        }
        triggeredMoves.Add(move);

        yield return null;

        if(triggeredMoves.Count == 0)
        {
            yield break;
        }
        if(triggeredMoves.Count >= 2)
        {
            move = ChooseMoveWithHighestPriority();
        }

        ClearAllMoves();
        triggeredMoves.Clear();

        if(move.powerMeterUsage > 0)
        {
            mainController.DepletePowerOrbMeter(move.powerMeterUsage);
        }
        if(move.vitalityMeterUsage > 0)
        {
            mainController.DepleteVitalityOrbMeter(move.vitalityMeterUsage);
        }
        Moves.MoveType moveType = move.moveType;
        
        switch (moveType)
        {
            case Moves.MoveType.Attack:
                bool isAirbourne = move.moveNotation.allowedState == "PlayerFallingState";
                bool canAttackInTheAir = mainController.attacksInTheAir >= 0;
                if (isAirbourne && canAttackInTheAir)
                {
                    mainController.StateMachine.ChangeState(new PlayerAirborneAttackState(mainController,
                    mainController.StateMachine, move));
                }
                else if (!isAirbourne)
                {
                    mainController.StateMachine.ChangeState(new PlayerAttackState(mainController,
                    mainController.StateMachine, move));
                }
                break;

            case Moves.MoveType.Projectile:
                bool _isAirbourne = move.moveNotation.allowedState == "PlayerFallingState";
                bool _canAttackInTheAir = mainController.attacksInTheAir >= 0;
                if (_isAirbourne && _canAttackInTheAir)
                {
                    mainController.StateMachine.ChangeState(new PlayerAirborneAttackState(mainController,
                    mainController.StateMachine, move));
                }
                else if (!_isAirbourne)
                {
                    mainController.StateMachine.ChangeState(new PlayerAttackState(mainController,
                    mainController.StateMachine, move));
                }
                break;
            case Moves.MoveType.Neutral:
                mainController.StateMachine.ChangeState(new PlayerNeutralMoveState(mainController,
                    mainController.StateMachine, move));
                break;
            
        }
        

        
        PlayerMoves ChooseMoveWithHighestPriority()
        {
            int biggestNotation = 0;
            PlayerMoves tempAttack = move;
            foreach (PlayerMoves tAttack in triggeredMoves)
            {
                if (tAttack.moveNotation.movementNotation.Length > biggestNotation)
                {
                    tempAttack = tAttack;
                    biggestNotation = tAttack.moveNotation.movementNotation.Length;
                }
            }

            return tempAttack;
        }

        bool CanExecutePowerMove(int amount)
        {
            if (amount > mainController.currentPowerOrbMeter)
                return false;
            return true;
        }

        bool CanExecuteVitalityMove(int amount)
        {
            if (amount > mainController.currentVitalityOrbMeter)
                return false;
            return true;
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
        
        foreach (PlayerMoves move in moveList.playerMoveList)
        {
            PlayerInputHandler.MovementInputNotation[] movementNotation = move.moveNotation.movementNotation;

            if (move.moveNotation.allowedState != CurrentStateOutput)
                continue;

            bool freeMovementMove = movementNotation.Length == 0;
            if (freeMovementMove)
            {
                StartCoroutine(inputHandler.InputCheck(move.moveNotation, move, this));
                continue;
            }

            bool firstNotationIsExecuted = movementNotation[0] == notation;

            if (inputHandler.HasLateralMovement(movementNotation[0]))
            {
                PlayerInputHandler.MovementInputNotation reversedNotation = inputHandler.ReverseInput(movementNotation[0]);
                bool firstNotationIsExecutedBackwards = reversedNotation == notation;

                if (firstNotationIsExecuted || firstNotationIsExecutedBackwards)
                {
                    if (!availableMoves.Contains(move.moveNotation))
                    {
                        StartCoroutine(inputHandler.InputCheckTimer(move.moveNotation, move, true, this));
                        StartCoroutine(inputHandler.InputCheckTimer(move.moveNotation, move, false, this));

                    }
                }
            }
            else if (inputHandler.HasLateralMovement(movementNotation))
            {
                if (firstNotationIsExecuted)
                {
                    if (!availableMoves.Contains(move.moveNotation))
                    {
                        StartCoroutine(inputHandler.InputCheckTimer(move.moveNotation, move, true, this));
                        StartCoroutine(inputHandler.InputCheckTimer(move.moveNotation, move, false, this));

                    }
                }
            }
            else if (firstNotationIsExecuted)
            {
                if (!availableMoves.Contains(move.moveNotation))
                {
                    StartCoroutine(inputHandler.InputCheckTimer(move.moveNotation, move, false, this));
                }
            }

        }
    }

    private IEnumerator LateStart()
    {
        yield return null;

        mainController.StateMachine.onStateChanged += UpdateMoveListOnStateChanged;
        inputHandler.onMovementCalled += UpdateMoveListOnInput;

        UpdateMoveListOnStateChanged("PlayerStandingState");
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        mainController.StateMachine.onStateChanged -= UpdateMoveListOnStateChanged;
        inputHandler.onMovementCalled -= UpdateMoveListOnInput;
    }
}
