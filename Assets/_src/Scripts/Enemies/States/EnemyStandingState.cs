using UnityEngine;
using System.Collections;

public class EnemyStandingState : EnemyGroundedState
{
    private float easingStandingMovementX;
    public EnemyStandingState(EnemyMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
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
            new Vector2(controllerScript.MovementX * controllerScript.enemySpeed, controllerScript.enemyRigidBody.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
