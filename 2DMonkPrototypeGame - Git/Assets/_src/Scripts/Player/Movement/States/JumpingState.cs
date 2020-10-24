using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class JumpingState : PlayerState
{
    #region Game Sound Names
    private const string S_PLAYER_JUMP = "player_jump";
    #endregion

    private float pastGravityScale;
    public JumpingState(PlayerMainController playerMovement, MainStateMachine stateMachine) : base(playerMovement, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        pastGravityScale = controllerScript.playerRigidBody.gravityScale;
        controllerScript.playerRigidBody.gravityScale = controllerScript.jumpSpeed;
        isGrounded = false;
        controllerScript.playerRigidBody.velocity = 
            Vector2.up * Mathf.Sqrt(controllerScript.jumpHeight * -2 * 
            Physics2D.gravity.y * controllerScript.playerRigidBody.gravityScale);
        controllerScript.hasPerformedJump?.Invoke();
        controllerScript.playerAnimationsScript.ChangeAnimationState("player_jump");
        controllerScript.jumpTimer = 0;

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
            stateMachine.ChangeState(new FallingState(controllerScript, stateMachine));
        }

        float tempSpeed = easingMovementX * controllerScript.moveSpeed;

        controllerScript.playerRigidBody.velocity =
            new Vector2(tempSpeed, controllerScript.playerRigidBody.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();
        controllerScript.playerRigidBody.gravityScale = pastGravityScale;
    }
}
