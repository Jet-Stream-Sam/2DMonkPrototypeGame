using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerState
{
    private MainVFXManager VFXManager;
    protected CancellationTokenSource tokenSource;
    private Vector3 initialPlayerScale;
    private bool lockAsyncMethod;
    private PlayerMoves attackAsset;
    private float attackTimer;

    protected IMoveBehaviour attackBehaviour;

    public PlayerAttackState(PlayerMainController controllerScript, MainStateMachine stateMachine,
       PlayerMoves playerAttackAsset) : base(controllerScript, stateMachine)
    {
        attackAsset = playerAttackAsset;
        if (playerAttackAsset.moveType == Moves.MoveType.Projectile)
        {
            controllerScript.AnimationEventWasCalled += ShootProjectile;
        }

        if (playerAttackAsset.moveBehaviour is IMoveBehaviour attack)
        {
            attackBehaviour = attack;
        }
        
    }


    public override void Enter()
    {
        base.Enter();

        SoundManager soundManager = SoundManager.Instance;

        VFXManager = controllerScript.playerMainVFXManager;
        initialPlayerScale = controllerScript.playerSpriteTransform.localScale;
        attackBehaviour?.Init(controllerScript, attackAsset, this);

        controllerScript.playerAnimationsScript.ChangeAnimationState(attackAsset.animationClip.name, false);

        if(attackAsset.moveSoundEffect != null)
            attackAsset.moveSoundEffect.PlaySound(soundManager, controllerScript.playerSpriteTransform.position);

        if (attackAsset.crySoundEffect != null)
            attackAsset.crySoundEffect.PlaySound(soundManager, controllerScript.playerSpriteTransform.position);

        controllerScript.hitBoxCheck.HitProperties = attackAsset.hitProperties;

        if(attackAsset.lockVelocity)
            LockVelocity();

        if (attackAsset.moveType == Moves.MoveType.Projectile && !attackAsset.attackAndProjectile)
            attackAsset.hitProperties = null;
        if (!lockAsyncMethod)
            AttackLoop();

        attackBehaviour?.OnMoveEnter();
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if(attackAsset.lockSideSwitch)
            LockSideSwitch(initialPlayerScale);
        attackBehaviour?.OnMoveUpdate();

        
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        attackBehaviour?.OnMoveFixedUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        if (attackAsset.moveType == Moves.MoveType.Projectile)
        {
            controllerScript.AnimationEventWasCalled -= ShootProjectile;
        }
        tokenSource.Cancel();
        controllerScript.hitBoxCheck.ResetProperties();
        attackBehaviour?.OnMoveExit();
    }
    private async void AttackLoop()
    {
        tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        lockAsyncMethod = true;

        float attackDuration = attackAsset.animationClip.length;

        attackTimer = attackDuration;

        while(attackTimer > 0)
        {
            if (attackAsset.moveNotation.needsToBeHeld)
            {
                if (controllerScript.playerInputHandler.RawButtonInput != attackAsset.moveNotation.buttonNotation)
                {
                    EndMoveAtState(attackAsset.moveEndsAtState);
                    return;
                }
            }
            attackTimer -= Time.deltaTime;
            await Task.Yield();
        }
     
        if (token.IsCancellationRequested)
            return;

        EndMoveAtState(attackAsset.moveEndsAtState);
    }
    private void EndMoveAtState(PlayerMoves.EndsAtState state)
    {
        if (controllerScript == null)
            return;

        switch (state)
        {
            case PlayerMoves.EndsAtState.Crouching:
                if (controllerScript.isGrounded)
                    stateMachine.ChangeState(new PlayerCrouchingState(controllerScript, stateMachine, 0.15f));
                else
                    stateMachine.ChangeState(new PlayerFallingState(controllerScript, stateMachine));
                break;
            default:
                if (controllerScript.isGrounded)
                    stateMachine.ChangeState(new PlayerStandingState(controllerScript, stateMachine));

                else
                    stateMachine.ChangeState(new PlayerFallingState(controllerScript, stateMachine));
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

    private void ShootProjectile(ScriptableObject obj)
    {
        if (!(obj is ProjectileTriggerEvent projEvent))
            return;
   
        var instantiatedObj = UnityEngine.Object.Instantiate(projEvent.fireballPrefab, 
            controllerScript.playerProjectileTransform.position, 
            Quaternion.identity, VFXManager.transform);
        var fireball = instantiatedObj.GetComponent<FireballBehaviour>();

        fireball.gameObject.transform.localScale = controllerScript.playerSpriteTransform.localScale.x >= 0 ?
            new Vector2(fireball.gameObject.transform.localScale.x, fireball.gameObject.transform.localScale.y) :
            new Vector2(-fireball.gameObject.transform.localScale.x, fireball.gameObject.transform.localScale.y);

    }
}
