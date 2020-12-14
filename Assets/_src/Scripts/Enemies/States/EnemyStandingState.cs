using UnityEngine;
using System.Collections;

public class EnemyStandingState : EnemyGroundedState
{
    public EnemyStandingState(EnemyMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        controllerScript.enemyAnimationsScript.ChangeAnimationState(
            controllerScript.enemyIdle.name, false);
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
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
