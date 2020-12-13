using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class PlayerNeutralMoveState : PlayerState
{
    protected CancellationTokenSource tokenSource;
    private Vector3 initialPlayerScale;
    private bool lockAsyncMethod;

    private PlayerMoves attackAsset;
    private string animationToPlay;
    private CollectionSounds audioClip;
    private bool lockVelocity;
    private bool lockSideSwitch;
    private float moveMaxDuration;
    private float moveTimer;
    private PlayerInputHandler.ButtonInputNotation buttonToHold;
    private bool buttonNeedsToBeHeld = false;
    private PlayerMoves.EndsAtState moveEndsAtState;
    protected IMoveBehaviour moveBehaviour;

    public PlayerNeutralMoveState(PlayerMainController controllerScript, MainStateMachine stateMachine,
       PlayerMoves playerAttackAsset) : base(controllerScript, stateMachine)
    {
        attackAsset = playerAttackAsset;
        animationToPlay = playerAttackAsset.animationClip.name;
        audioClip = playerAttackAsset.moveSoundEffect;
        lockVelocity = playerAttackAsset.lockVelocity;
        lockSideSwitch = playerAttackAsset.lockSideSwitch;
        moveEndsAtState = playerAttackAsset.moveEndsAtState;
        moveMaxDuration = playerAttackAsset.animationClip.length;
        buttonToHold = playerAttackAsset.moveNotation.buttonNotation;
        buttonNeedsToBeHeld = playerAttackAsset.moveNotation.needsToBeHeld;

        if (playerAttackAsset.moveBehaviour is IMoveBehaviour move)
        {
            moveBehaviour = move;
        }

    }

    public override void Enter()
    {
        base.Enter();
        moveBehaviour?.Init(controllerScript, attackAsset, this);

        initialPlayerScale = controllerScript.playerSpriteTransform.localScale;

        controllerScript.playerAnimationsScript.ChangeAnimationState(animationToPlay);

        SoundManager soundManager = SoundManager.Instance;

        if(audioClip != null)
            audioClip.PlaySound(soundManager);

        if (lockVelocity)
            LockVelocity();

        if (!lockAsyncMethod)
            MoveLoop();

        moveBehaviour?.OnMoveEnter();
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if (lockSideSwitch)
            LockSideSwitch(initialPlayerScale);
        moveBehaviour?.OnMoveUpdate();
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        moveBehaviour?.OnMoveFixedUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        tokenSource.Cancel();
        moveBehaviour?.OnMoveExit();
    }

    private async void MoveLoop()
    {
        tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        lockAsyncMethod = true;

        moveTimer = moveMaxDuration;

        while(moveTimer > 0)
        {
            if (buttonNeedsToBeHeld)
            {
                if (controllerScript.playerInputHandler.RawButtonInput != buttonToHold)
                {
                    Debug.Log("Move was interrupted!");
                    EndMoveAtState(moveEndsAtState);
                    return;
                }
            }
            moveTimer -= Time.deltaTime;
            await Task.Yield();
        }
        if (token.IsCancellationRequested)
            return;

        Debug.Log("Move was executed till the end!");
        EndMoveAtState(moveEndsAtState);
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
}
