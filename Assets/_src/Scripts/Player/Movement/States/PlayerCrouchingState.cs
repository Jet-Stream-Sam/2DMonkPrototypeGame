using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class PlayerCrouchingState : PlayerGroundedState
{
    private float cooldown = 0;
    private bool canTransition = true;
    private float easingStandingMovementX;
    public PlayerCrouchingState(PlayerMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {

    }

    public PlayerCrouchingState(PlayerMainController controllerScript, MainStateMachine stateMachine,
        float cooldownTillNextTransition) : base(controllerScript, stateMachine)
    {
        cooldown = cooldownTillNextTransition;
    }


    public override void Enter()
    {
        base.Enter();
        #region Input Handling

        controllerScript.Controls.Player.Kick.started -= controllerScript.kickAction;
        controllerScript.Controls.Player.Punch.started -= controllerScript.punchAction;
        controllerScript.kickAction = _ => stateMachine.ChangeState(new PlayerAttackState(controllerScript, stateMachine,
           controllerScript.playerMoveList.Find("player_crouching_kick")));

        #endregion

        controllerScript.playerAnimationsScript.ChangeAnimationState("player_crouch", false);

        if (cooldown > 0)
        {
            canTransition = false;
            CountDown(cooldown);
            
        }
    }
    public override void HandleUpdate()
    {
        base.HandleUpdate();
        easingStandingMovementX =
            Mathf.Lerp(easingStandingMovementX,
            controllerScript.MovementX,
            controllerScript.standingEasingRate);

        easingStandingMovementX =
            ClampMovement(easingStandingMovementX);

        if (!canTransition && controllerScript.MovementY > deadzoneMin && !controllerScript.isHittingHead)
        {
            stateMachine.ChangeState(new PlayerStandingState(controllerScript, stateMachine));
        }
        else if (canTransition && !controllerScript.isHittingHead)
        {
            if (controllerScript.MovementY == 0)
            {
                stateMachine.ChangeState(new PlayerStandingState(controllerScript, stateMachine));
            }
       
        }
        
        if (controllerScript.airborneJumpTimer > Time.time && !controllerScript.isHittingHead)
        {
            stateMachine.ChangeState(new PlayerJumpingState(controllerScript, stateMachine));
        }

    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();

        float tempSpeed = easingStandingMovementX * controllerScript.crouchingMoveSpeed;

        controllerScript.playerRigidBody.velocity =
            new Vector2(tempSpeed, controllerScript.playerRigidBody.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();  
    }

    private async void CountDown(float value)
    {
        float countSeconds = value;
        while (countSeconds > 0)
        {
            countSeconds -= Time.deltaTime;

            await Task.Yield();
        }

        canTransition = true;
    }
}
