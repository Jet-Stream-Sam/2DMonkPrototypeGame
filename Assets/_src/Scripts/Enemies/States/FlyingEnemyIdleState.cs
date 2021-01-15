using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyIdleState : FlyingEnemyState
{
    public FlyingEnemyIdleState(EnemyMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        controllerScript.enemyAnimationsScript.ChangeAnimationState(
            controllerScript.idleAnimationClip.name, false);
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
