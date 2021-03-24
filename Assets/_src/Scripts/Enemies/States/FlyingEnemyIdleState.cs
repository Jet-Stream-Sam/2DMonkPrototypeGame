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

        controllerScript.enemyRigidBody.velocity =
            new Vector2(controllerScript.MovementX * controllerScript.enemySpeed, controllerScript.MovementY * controllerScript.enemySpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
