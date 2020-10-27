using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class StandingAttackState : GroundedState
{
    private Vector3 initialPlayerScale;
    private bool lockAsyncMethod;

    private string animationToPlay;
    private string audioClipName;
    private bool lockVelocity;
    private bool lockSideSwitch;

    private int damage;

    public StandingAttackState(PlayerMainController controllerScript, MainStateMachine stateMachine,
       PlayerAttack playerAttackAsset) : base(controllerScript, stateMachine)
    {
        animationToPlay = playerAttackAsset.animationName;
        audioClipName = playerAttackAsset.audioClipName;
        lockVelocity = playerAttackAsset.lockVelocity;
        lockSideSwitch = playerAttackAsset.lockSideSwitch;
        damage = playerAttackAsset.damage;
    }

    public override void Enter()
    {
        base.Enter();
        
        controllerScript.playerAnimationsScript.ChangeAnimationState(animationToPlay);
        controllerScript.SoundManager.PlayOneShotSFX(audioClipName);

        controllerScript.hitBoxCheck.Damage = damage;

        initialPlayerScale = controllerScript.playerSpriteTransform.localScale;
        if(lockVelocity)
            LockVelocity();
        
        if (!lockAsyncMethod)
            AttackLoop();

    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if(lockSideSwitch)
            LockSideSwitch(initialPlayerScale);
    }
    public override void Exit()
    {
        base.Exit();
        controllerScript.hitBoxCheck.Damage = 0;
    }
    private async void AttackLoop()
    {
        lockAsyncMethod = true;
        await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime + 0.01f));

        await Task.Delay(TimeSpan.FromSeconds
            (controllerScript.playerAnimationsScript.GetCurrentAnimationLength()));

        
        stateMachine.ChangeState(new StandingState(controllerScript, stateMachine));
        
    }

    private void LockVelocity()
    {
        controllerScript.playerRigidBody.velocity = new Vector2(0, 0);
    }

    private void LockSideSwitch(Vector3 initialScale)
    {
        controllerScript.playerSpriteTransform.localScale = initialScale;
    }
}
