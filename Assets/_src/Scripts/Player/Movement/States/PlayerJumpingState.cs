using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerJumpingState : PlayerState
{
    #region Game Sound Names
    private const string S_PLAYER_JUMP = "Player Jump";
    #endregion

    private float pastGravityScale;
    public PlayerJumpingState(PlayerMainController playerMovement, MainStateMachine stateMachine) : base(playerMovement, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        ParticleSystem jumpDust = controllerScript.playerMainVFXManager.playerDustParticles.dustParticles["JumpDust"];
        jumpDust.Play();

        controllerScript.playerAnimationsScript.ChangeAnimationState("player_fall", false);

        pastGravityScale = controllerScript.playerRigidBody.gravityScale;

        controllerScript.playerRigidBody.gravityScale = controllerScript.jumpSpeed;

        controllerScript.isGrounded = false;

        controllerScript.playerRigidBody.velocity = 
            Vector2.up * Mathf.Sqrt(controllerScript.jumpHeight * -2 * 
            Physics2D.gravity.y * controllerScript.playerRigidBody.gravityScale);

        controllerScript.hasPerformedJump?.Invoke();

        controllerScript.playerAnimationsScript.ChangeAnimationState("player_jump", false);

        controllerScript.airborneJumpTimer = 0;

        controllerScript.SoundManager.PlaySFX(S_PLAYER_JUMP);
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        if (controllerScript.playerRigidBody.velocity.y > 0 && !controllerScript.IsHoldingJumpButton)
        {

            controllerScript.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * controllerScript.fallMultiplier * Time.deltaTime;

        }

        if(controllerScript.playerRigidBody.velocity.y < 0)
        {
            stateMachine.ChangeState(new PlayerFallingState(controllerScript, stateMachine));
        }

        float tempSpeed = easingMovementX * controllerScript.standingMoveSpeed;

        controllerScript.playerRigidBody.velocity =
            new Vector2(tempSpeed, controllerScript.playerRigidBody.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();
        controllerScript.groundedJumpTimer = 0;
        controllerScript.playerRigidBody.gravityScale = pastGravityScale;
    }
}
