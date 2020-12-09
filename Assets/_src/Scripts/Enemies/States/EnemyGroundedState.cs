using UnityEngine;
using System.Collections;

public class EnemyGroundedState : EnemyState
{
    public EnemyGroundedState(EnemyMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if (!controllerScript.isGrounded)
            stateMachine.ChangeState(new EnemyFallingState(controllerScript, stateMachine));
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
