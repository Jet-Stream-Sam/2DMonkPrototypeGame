using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHitStunnedState : PlayerState
{
    private Vector3 initialPlayerScale;
    private float easingMovementX;

    public PlayerHitStunnedState(PlayerMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        
        initialPlayerScale = controllerScript.playerSpriteTransform.localScale;
        controllerScript.playerAnimationsScript.ChangeAnimationState("player_hit_stunned", true);
        easingMovementX = controllerScript.playerRigidBody.velocity.x;

        if (controllerScript.stunnedCoroutine != null)
            controllerScript.StopCoroutine(controllerScript.stunnedCoroutine);
        controllerScript.StartCoroutine(controllerScript.stunnedCoroutine = ComeBackToState(controllerScript.stunnedMaxTime));
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();

        controllerScript.playerSpriteTransform.localScale = initialPlayerScale;
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();

        if (controllerScript.isGrounded)
        {
            easingMovementX =
            Mathf.Lerp(easingMovementX,
            0,
            controllerScript.groundedStunnedToIdleEasingRate);

            controllerScript.playerRigidBody.velocity =
                new Vector2(easingMovementX, controllerScript.playerRigidBody.velocity.y);
        }
        else
        {
            float gravity = Physics2D.gravity.y * controllerScript.fallMultiplier * Time.deltaTime;

            easingMovementX =
            Mathf.Lerp(easingMovementX,
            0,
            controllerScript.airborneStunnedToIdleEasingRate);

            controllerScript.playerRigidBody.velocity =
                new Vector2(easingMovementX, controllerScript.playerRigidBody.velocity.y + gravity);
  
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


    private IEnumerator ComeBackToState(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            Debug.Log(time);
            yield return null;
        }
            

        if (controllerScript.isHittingHead)
            stateMachine.ChangeState(new PlayerCrouchingState(controllerScript, stateMachine));
        else
            stateMachine.ChangeState(new PlayerStandingState(controllerScript, stateMachine));
    }
}
