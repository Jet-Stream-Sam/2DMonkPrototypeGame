using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StandingState : GroundedState
{
    public StandingState(PlayerMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        #region Input Handling

        controllerScript.kickAction = _ => stateMachine.ChangeState(new AttackState(controllerScript, stateMachine,
            controllerScript.playerMoveList.Find("player_kick")));
        controllerScript.punchAction = _ => stateMachine.ChangeState(new AttackState(controllerScript, stateMachine,
            controllerScript.playerMoveList.Find("player_punch")));

        controllerScript.InputSubscribe();

        #endregion

        
        if (controllerScript.MovementX != 0)
            controllerScript.playerAnimationsScript.ChangeAnimationState("player_walk");
        else
            controllerScript.playerAnimationsScript.ChangeAnimationState("player_idle");
    }

    public override void HandleUpdate()
    {
        if (controllerScript.MovementX != 0)
            controllerScript.playerAnimationsScript.ChangeAnimationState("player_walk");
        else
            controllerScript.playerAnimationsScript.ChangeAnimationState("player_idle");

        base.HandleUpdate();


        if (controllerScript.MovementY < -0.5f)
        {
            stateMachine.ChangeState(new CrouchingState(controllerScript, stateMachine));
        }
        if (controllerScript.airborneJumpTimer > Time.time)
            stateMachine.ChangeState(new JumpingState(controllerScript, stateMachine));

    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();

        float tempSpeed = easingMovementX * controllerScript.standingMoveSpeed;

        controllerScript.playerRigidBody.velocity =
            new Vector2(tempSpeed, controllerScript.playerRigidBody.velocity.y);

        
    }

    public override void Exit()
    {
        base.Exit();


    }

    
    
    
}
