using UnityEngine;
using System.Collections;

public class EnemyHitStunnedState : EnemyState
{
    private float easingMovementX;
    public EnemyHitStunnedState(EnemyMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controllerScript.enemyAnimationsScript.ChangeAnimationState(controllerScript.hitAnimationClip.name, true);
        controllerScript.AIBrain.StateReset();
        controllerScript.StartCoroutine(ComeBackToState(controllerScript.hitAnimationClip.length));

        if (controllerScript.stunnedCoroutine != null)
            controllerScript.StopCoroutine(controllerScript.stunnedCoroutine);
        controllerScript.StartCoroutine(controllerScript.stunnedCoroutine = ComeBackToState(controllerScript.stunnedMaxTime));
        easingMovementX = controllerScript.enemyRigidBody.velocity.x;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();

    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        if (controllerScript.isGrounded)
        {
            easingMovementX =
            Mathf.Lerp(easingMovementX,
            0,
            controllerScript.groundedStunnedToIdleEasingRate);

            controllerScript.enemyRigidBody.velocity =
                new Vector2(easingMovementX, controllerScript.enemyRigidBody.velocity.y);
        }
        else
        {
            easingMovementX =
            Mathf.Lerp(easingMovementX,
            0,
            controllerScript.airborneStunnedToIdleEasingRate);

            controllerScript.enemyRigidBody.velocity =
                new Vector2(easingMovementX, controllerScript.enemyRigidBody.velocity.y);

        }
    }

    public override void Exit()
    {
        base.Exit();
    }


    private IEnumerator ComeBackToState(float time)
    {
        yield return new WaitForSeconds(time);

        controllerScript.enemyRigidBody.velocity = Vector2.zero;
        stateMachine.ChangeState(new EnemyStandingState(controllerScript, stateMachine));

    }
}
