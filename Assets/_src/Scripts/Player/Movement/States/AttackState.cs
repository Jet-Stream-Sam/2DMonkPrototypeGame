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
    private float attackDuration;
    private PlayerAttack.EndsAtState attackEndsAtState;
    protected IAttackBehaviour attackBehaviour;

    private int damage;

    public AttackState(PlayerMainController controllerScript, MainStateMachine stateMachine,
       PlayerAttack playerAttackAsset) : base(controllerScript, stateMachine)
    {
        animationToPlay = playerAttackAsset.animationClip.name;
        audioClipName = playerAttackAsset.audioClip.name;
        lockVelocity = playerAttackAsset.lockVelocity;
        lockSideSwitch = playerAttackAsset.lockSideSwitch;
        damage = playerAttackAsset.damage;
        attackEndsAtState = playerAttackAsset.attackEndsAtState;
        attackDuration = playerAttackAsset.animationClip.length;

        if (playerAttackAsset.attackBehaviour is IAttackBehaviour attack)
        {
            attackBehaviour = attack;
        }
    }


    public override void Enter()
    {
        base.Enter();
        attackBehaviour?.Init(controllerScript);
        


        controllerScript.playerAnimationsScript.ChangeAnimationState(animationToPlay);
        controllerScript.SoundManager.PlayOneShotSFX(audioClipName);

        controllerScript.hitBoxCheck.Damage = damage;

        initialPlayerScale = controllerScript.playerSpriteTransform.localScale;
        if(lockVelocity)
            LockVelocity();
        
        if (!lockAsyncMethod)
            AttackLoop();

        attackBehaviour?.OnAttackEnter();
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if(lockSideSwitch)
            LockSideSwitch(initialPlayerScale);
        attackBehaviour?.OnAttackUpdate();
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        attackBehaviour?.OnAttackFixedUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        controllerScript.hitBoxCheck.Damage = 0;
        attackBehaviour?.OnAttackExit();
    }
    private async void AttackLoop()
    {
        tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        lockAsyncMethod = true;

        await Task.Delay(TimeSpan.FromSeconds
            (attackDuration));

        if (token.IsCancellationRequested)
            return;
        switch (attackEndsAtState)
        {
            case PlayerAttack.EndsAtState.Crouching:
                if(controllerScript.isGrounded)
                    stateMachine.ChangeState(new CrouchingState(controllerScript, stateMachine, 0.15f));
                else
                    stateMachine.ChangeState(new FallingState(controllerScript, stateMachine));
                break;
            default:
                if (controllerScript.isGrounded)
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
