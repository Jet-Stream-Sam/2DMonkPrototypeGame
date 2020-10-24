using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingState : GroundedState
{
    public StandingState(PlayerMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void HandleUpdate()
    {
        if (controllerScript.MovementX != 0)
            controllerScript.playerAnimationsScript.ChangeAnimationState("player_walk");
        else
            controllerScript.playerAnimationsScript.ChangeAnimationState("player_idle");

        base.HandleUpdate();

        if (controllerScript.jumpTimer > Time.time)
            stateMachine.ChangeState(new JumpingState(controllerScript, stateMachine));

    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();

        float tempSpeed = easingMovementX * controllerScript.moveSpeed;

        controllerScript.playerRigidBody.velocity =
            new Vector2(tempSpeed, controllerScript.playerRigidBody.velocity.y);

        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
