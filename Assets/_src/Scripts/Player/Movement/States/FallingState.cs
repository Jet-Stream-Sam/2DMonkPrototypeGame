using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class FallingState : PlayerState
{
    private Action<InputAction.CallbackContext> kickAction;
    public FallingState(PlayerMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        controllerScript.playerAnimationsScript.ChangeAnimationState("player_fall");
        #region Input Handling
        controllerScript.Controls.Player.Kick.started -= kickAction;
        kickAction = _ => stateMachine.ChangeState(new AirborneAttackState(controllerScript, stateMachine,
           controllerScript.playerMoveList.Find("player_airborne_kick")));
        controllerScript.Controls.Player.Kick.started += kickAction;
        #endregion

    }
    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if (isGrounded) stateMachine.ChangeState(new StandingState(controllerScript, stateMachine));

    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        controllerScript.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * controllerScript.fallMultiplier * Time.deltaTime;

        float tempSpeed = easingMovementX * controllerScript.standingMoveSpeed;

        controllerScript.playerRigidBody.velocity =
            new Vector2(tempSpeed, controllerScript.playerRigidBody.velocity.y);
    }
    public override void Exit()
    {
        controllerScript.Controls.Player.Kick.started -= kickAction;
        base.Exit();
    }
}
