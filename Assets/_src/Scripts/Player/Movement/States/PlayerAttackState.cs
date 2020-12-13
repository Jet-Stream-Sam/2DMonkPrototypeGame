using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerState
{
    protected CancellationTokenSource tokenSource;
    private Vector3 initialPlayerScale;
    private bool lockAsyncMethod;

    private PlayerMoves attackAsset;
    private string animationToPlay;
    private CollectionSounds audioClip;
    private bool lockVelocity;
    private bool lockSideSwitch;
    private float attackDuration;
    private float attackTimer;
    private HitProperties hitProperties;
    private PlayerInputHandler.ButtonInputNotation buttonToHold;
    private bool buttonNeedsToBeHeld = false;
    private PlayerMoves.EndsAtState attackEndsAtState;
    protected IMoveBehaviour attackBehaviour;
    protected Moves.MoveType moveType;
    protected bool attackAndProjectile;

    public PlayerAttackState(PlayerMainController controllerScript, MainStateMachine stateMachine,
       PlayerMoves playerAttackAsset) : base(controllerScript, stateMachine)
    {
        attackAsset = playerAttackAsset;
        animationToPlay = playerAttackAsset.animationClip.name;
        audioClip = playerAttackAsset.moveSoundEffect;
        lockVelocity = playerAttackAsset.lockVelocity;
        lockSideSwitch = playerAttackAsset.lockSideSwitch;
        hitProperties = playerAttackAsset.hitProperties;
        attackEndsAtState = playerAttackAsset.moveEndsAtState;
        attackDuration = playerAttackAsset.animationClip.length;
        buttonToHold = playerAttackAsset.moveNotation.buttonNotation;
        buttonNeedsToBeHeld = playerAttackAsset.moveNotation.needsToBeHeld;
        moveType = playerAttackAsset.moveType;

        if (moveType == Moves.MoveType.Projectile)
        {
            attackAndProjectile = playerAttackAsset.attackAndProjectile;
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
        attackBehaviour?.Init(controllerScript, attackAsset, this);

        initialPlayerScale = controllerScript.playerSpriteTransform.localScale;

        controllerScript.playerAnimationsScript.ChangeAnimationState(animationToPlay);

        SoundManager soundManager = SoundManager.Instance;

        if(audioClip != null)
            audioClip.PlaySound(soundManager);

        controllerScript.hitBoxCheck.HitProperties = 
            new HitProperties(hitProperties);

        if(lockVelocity)
            LockVelocity();

        if (moveType == Moves.MoveType.Projectile && !attackAndProjectile)
            hitProperties = null;
        if (!lockAsyncMethod)
            AttackLoop();

        attackBehaviour?.OnMoveEnter();
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if(lockSideSwitch)
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
        if (moveType == Moves.MoveType.Projectile)
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

        attackTimer = attackDuration;

        while(attackTimer > 0)
        {
            if (buttonNeedsToBeHeld)
            {
                if (controllerScript.playerInputHandler.RawButtonInput != buttonToHold)
                {
                    EndMoveAtState(attackEndsAtState);
                    return;
                }
            }
            attackTimer -= Time.deltaTime;
            await Task.Yield();
        }
     
        if (token.IsCancellationRequested)
            return;

        EndMoveAtState(attackEndsAtState);
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

        
        GameObject instantiatedObj = UnityEngine.Object.Instantiate(projEvent.fireballPrefab, controllerScript.playerProjectileTransform.position, Quaternion.identity);
        FireballBehaviour fireball = instantiatedObj.GetComponent<FireballBehaviour>();

        fireball.gameObject.transform.localScale = controllerScript.playerSpriteTransform.localScale.x >= 0 ?
            new Vector2(fireball.gameObject.transform.localScale.x, fireball.gameObject.transform.localScale.y) :
            new Vector2(-fireball.gameObject.transform.localScale.x, fireball.gameObject.transform.localScale.y);

    }
}
