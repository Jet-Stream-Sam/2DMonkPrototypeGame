using UnityEngine;
using System;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class CrouchingState : GroundedState
{
    private Action<InputAction.CallbackContext> kickAction;
    private float cooldown = 0;
    private bool canTransition = true;
    public CrouchingState(PlayerMainController controllerScript, MainStateMachine stateMachine) : base(controllerScript, stateMachine)
    {

    }

    public CrouchingState(PlayerMainController controllerScript, MainStateMachine stateMachine,
        float cooldownTillNextTransition) : base(controllerScript, stateMachine)
    {
        cooldown = cooldownTillNextTransition;
    }


    public override void Enter()
    {
        base.Enter();
        #region Input Handling
        controllerScript.Controls.Player.Kick.started -= kickAction;
        kickAction = _ => stateMachine.ChangeState(new AttackState(controllerScript, stateMachine,
           controllerScript.playerMoveList.Find("player_crouching_kick")));
        controllerScript.Controls.Player.Kick.started += kickAction;
        #endregion

        controllerScript.playerAnimationsScript.ChangeAnimationState("player_crouch");
        controllerScript.playerMainCollider.offset = controllerScript.crouchingColliderOffset;
        controllerScript.playerMainCollider.size = controllerScript.crouchingColliderSize;

        if (cooldown > 0)
        {
            canTransition = false;
            CountDown(cooldown);
            
        }
    }
    public override void HandleUpdate()
    {
        base.HandleUpdate();
        if (!canTransition && controllerScript.MovementY > 0.5f)
        {
            stateMachine.ChangeState(new StandingState(controllerScript, stateMachine));
            controllerScript.playerMainCollider.offset = controllerScript.standingColliderOffset;
            controllerScript.playerMainCollider.size = controllerScript.standingColliderSize;
        }
        else if (canTransition)
        {
            if (controllerScript.MovementY == 0)
            {
                stateMachine.ChangeState(new StandingState(controllerScript, stateMachine));
                controllerScript.playerMainCollider.offset = controllerScript.standingColliderOffset;
                controllerScript.playerMainCollider.size = controllerScript.standingColliderSize;
            }

            
                
        }
        
        if (controllerScript.jumpTimer > Time.time)
        {
            stateMachine.ChangeState(new JumpingState(controllerScript, stateMachine));
            controllerScript.playerMainCollider.offset = controllerScript.standingColliderOffset;
            controllerScript.playerMainCollider.size = controllerScript.standingColliderSize;
        }

    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();

        float tempSpeed = easingMovementX * controllerScript.crouchingMoveSpeed;

        controllerScript.playerRigidBody.velocity =
            new Vector2(tempSpeed, controllerScript.playerRigidBody.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();
        controllerScript.Controls.Player.Kick.started -= kickAction;

        
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
