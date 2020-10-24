using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : PlayerState
{
    public FallingState(PlayerMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        controllerScript.playerAnimationsScript.ChangeAnimationState("player_fall");
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

        float tempSpeed = easingMovementX * controllerScript.moveSpeed;

        controllerScript.playerRigidBody.velocity =
            new Vector2(tempSpeed, controllerScript.playerRigidBody.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
