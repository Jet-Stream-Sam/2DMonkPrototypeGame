using UnityEngine;
using System.Collections;

public class EnemyHitStunnedState : EnemyState
{
    public EnemyHitStunnedState(EnemyMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controllerScript.enemyAnimationsScript.ChangeAnimationState(controllerScript.hitAnimationClip.name);
        controllerScript.StartCoroutine(ComeBackToState(controllerScript.enemyIdle.name,
            controllerScript.hitAnimationClip.length));
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();

        if (controllerScript.isGrounded && controllerScript.hasRecovered)
        {
            bool hasStopped = Mathf.Abs(controllerScript.enemyRigidBody.velocity.x) < 0.01f && controllerScript.enemyRigidBody.velocity.y < 0.01f;
            if (hasStopped)
            {
                controllerScript.hasNormalizedMovement = true;
                controllerScript.enemyRigidBody.velocity = Vector2.zero;
                stateMachine.ChangeState(new EnemyStandingState(controllerScript, stateMachine));
            }

        }

        
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        if (controllerScript.isGrounded && !controllerScript.hasNormalizedMovement)
        {
            controllerScript.enemyRigidBody.velocity = 
                Vector2.Lerp(controllerScript.enemyRigidBody.velocity, Vector2.zero,
                controllerScript.groundedStunnedToIdleEasingRate);
        }
        else if (!controllerScript.isGrounded && !controllerScript.hasRecovered || 
            !controllerScript.isGrounded && !controllerScript.hasNormalizedMovement)
        {
            controllerScript.enemyRigidBody.velocity =
                Vector2.Lerp(controllerScript.enemyRigidBody.velocity, Vector2.zero, 
                controllerScript.airborneStunnedToIdleEasingRate);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


    private IEnumerator ComeBackToState(string state, float time)
    {
        controllerScript.hasRecovered = false;
        controllerScript.hasNormalizedMovement = false;
        yield return new WaitForSeconds(time);
        controllerScript.hasRecovered = true;
        controllerScript.enemyAnimationsScript.ChangeAnimationState(state);
    }
}
