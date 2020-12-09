using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void HandleUpdate()
    {
        base.HandleUpdate();

        controllerScript.groundedJumpTimer = Time.time + controllerScript.groundedJumpDelay;
        if (!controllerScript.isGrounded)
            stateMachine.ChangeState(new PlayerFallingState(controllerScript, stateMachine));

    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
