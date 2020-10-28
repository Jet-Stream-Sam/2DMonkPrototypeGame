using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingAttackState : AttackState
{
    public CrouchingAttackState(PlayerMainController controllerScript, MainStateMachine stateMachine, PlayerAttack playerAttackAsset) : base(controllerScript, stateMachine, playerAttackAsset)
    {

    }
    public override void Enter()
    {
        base.Enter();
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
        controllerScript.playerMainCollider.offset = controllerScript.standingColliderOffset;
        controllerScript.playerMainCollider.size = controllerScript.standingColliderSize;
    }
}
