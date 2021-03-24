using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyHitStunnedState : FlyingEnemyState
{
    public FlyingEnemyHitStunnedState(EnemyMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controllerScript.enemyAnimationsScript.ChangeAnimationState(controllerScript.hitAnimationClip.name, true);
        controllerScript.AIBrain.StateReset();
        controllerScript.StartCoroutine(ComeBackToState(controllerScript.hitAnimationClip.length));
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();

    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();

        controllerScript.enemyRigidBody.velocity =
                Vector2.Lerp(controllerScript.enemyRigidBody.velocity, Vector2.zero,
                controllerScript.airborneStunnedToIdleEasingRate);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator ComeBackToState(float time)
    {
        yield return new WaitForSeconds(time);

        controllerScript.enemyRigidBody.velocity = Vector2.zero;
        stateMachine.ChangeState(new FlyingEnemyIdleState(controllerScript, stateMachine));

    }
}
