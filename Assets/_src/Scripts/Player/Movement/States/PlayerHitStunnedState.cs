using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHitStunnedState : PlayerState
{
    private Vector3 initialPlayerScale;
    public PlayerHitStunnedState(PlayerMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        initialPlayerScale = controllerScript.playerSpriteTransform.localScale;
        controllerScript.playerAnimationsScript.ChangeAnimationState("player_hit_stunned", true);
        controllerScript.StartCoroutine(ComeBackToState(0.2f));
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();

        if (controllerScript.isGrounded && controllerScript.hasRecovered)
        {
            bool hasStopped = Mathf.Abs(controllerScript.playerRigidBody.velocity.x) < 0.01f && controllerScript.playerRigidBody.velocity.y < 0.01f;
            if (hasStopped)
            {
                controllerScript.hasNormalizedMovement = true;
                controllerScript.playerRigidBody.velocity = Vector2.zero;

                if(controllerScript.isHittingHead)
                    stateMachine.ChangeState(new PlayerCrouchingState(controllerScript, stateMachine));
                else
                    stateMachine.ChangeState(new PlayerStandingState(controllerScript, stateMachine));
            }

        }

        controllerScript.playerSpriteTransform.localScale = initialPlayerScale;
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();

        

        

        if (controllerScript.isGrounded && !controllerScript.hasNormalizedMovement)
        {
            controllerScript.playerRigidBody.velocity =
                Vector2.Lerp(controllerScript.playerRigidBody.velocity, Vector2.zero,
                controllerScript.groundedStunnedToIdleEasingRate);
        }
        else if (!controllerScript.isGrounded && !controllerScript.hasRecovered ||
            !controllerScript.isGrounded && !controllerScript.hasNormalizedMovement)
        {
            controllerScript.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * controllerScript.fallMultiplier * Time.deltaTime;

            controllerScript.playerRigidBody.velocity =
                new Vector2(0, controllerScript.playerRigidBody.velocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


    private IEnumerator ComeBackToState(float time)
    {
        controllerScript.hasRecovered = false;
        controllerScript.hasNormalizedMovement = false;
        yield return new WaitForSeconds(time);
        controllerScript.hasRecovered = true;
        
    }
}
