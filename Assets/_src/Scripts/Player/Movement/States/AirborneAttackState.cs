using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneAttackState : AttackState
{
    public AirborneAttackState(PlayerMainController controllerScript, MainStateMachine stateMachine, PlayerAttack playerAttackAsset) : base(controllerScript, stateMachine, playerAttackAsset)
    {

    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if (isGrounded) stateMachine.ChangeState(new StandingState(controllerScript, stateMachine));

    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        controllerScript.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * controllerScript.fallMultiplier * Time.deltaTime;

        float tempSpeed = easingMovementX * controllerScript.standingMoveSpeed;

        controllerScript.playerRigidBody.velocity =
            new Vector2(tempSpeed, controllerScript.playerRigidBody.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();
        
    }
}
