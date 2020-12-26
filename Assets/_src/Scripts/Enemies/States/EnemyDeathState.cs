using UnityEngine;
using System.Collections;

public class EnemyDeathState : EnemyState
{
    public EnemyDeathState(EnemyMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        OverridingState = true;
        controllerScript.enemyAnimationsScript.ChangeAnimationState(
            controllerScript.deathAnimationClip.name, false);
        controllerScript.enemyCollider.enabled = false;
        controllerScript.enemyRigidBody.isKinematic = true;
        controllerScript.enemyRigidBody.Sleep();
        Object.Destroy(controllerScript.gameObject, controllerScript.deathAnimationClip.length); 
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
