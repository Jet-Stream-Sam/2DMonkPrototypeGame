using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class AttackState : PlayerState
{
    protected CancellationTokenSource tokenSource;
    private Vector3 initialPlayerScale;
    private bool lockAsyncMethod;

    private string animationToPlay;
    private string audioClipName;
    private bool lockVelocity;
    private bool lockSideSwitch;
    private PlayerAttack.EndsAtState attackEndsAtState;

    private int damage;

    public AttackState(PlayerMainController controllerScript, MainStateMachine stateMachine,
       PlayerAttack playerAttackAsset) : base(controllerScript, stateMachine)
    {
        animationToPlay = playerAttackAsset.animationName;
        audioClipName = playerAttackAsset.audioClipName;
        lockVelocity = playerAttackAsset.lockVelocity;
        lockSideSwitch = playerAttackAsset.lockSideSwitch;
        damage = playerAttackAsset.damage;
        attackEndsAtState = playerAttackAsset.attackEndsAtState;
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
        tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        lockAsyncMethod = true;
        await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime + 0.01f));

        
        await Task.Delay(TimeSpan.FromSeconds
            (controllerScript.playerAnimationsScript.GetCurrentAnimationLength() - 0.01f));

        if (token.IsCancellationRequested)
            return;
        switch (attackEndsAtState)
        {
            case PlayerAttack.EndsAtState.Crouching:
                if(isGrounded)
                    stateMachine.ChangeState(new CrouchingState(controllerScript, stateMachine, 0.15f));
                else
                    stateMachine.ChangeState(new FallingState(controllerScript, stateMachine));
                break;
            default:
                if (isGrounded)
                    stateMachine.ChangeState(new StandingState(controllerScript, stateMachine));
                else
                    stateMachine.ChangeState(new FallingState(controllerScript, stateMachine));
                break;

        }
        
        
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
