using UnityEngine;
using System.Collections;

public class EnemyFallingState : EnemyState
{
    public EnemyFallingState(EnemyMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controllerScript.enemyAnimationsScript.ChangeAnimationState(controllerScript.fallAnimationClip.name, false);
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        bool hasLanded = controllerScript.isGrounded;
        if (hasLanded)
        {
            stateMachine.ChangeState(new EnemyStandingState(controllerScript, stateMachine));
        }
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
