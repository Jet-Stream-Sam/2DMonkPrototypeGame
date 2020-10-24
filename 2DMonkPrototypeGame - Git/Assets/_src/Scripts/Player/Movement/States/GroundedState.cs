using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerState
{
    public GroundedState(PlayerMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void HandleUpdate()
    {
        base.HandleUpdate();

        if (!isGrounded)
            stateMachine.ChangeState(new FallingState(controllerScript, stateMachine));

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
