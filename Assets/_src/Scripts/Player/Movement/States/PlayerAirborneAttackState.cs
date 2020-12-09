using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneAttackState : PlayerAttackState
{
    
    public PlayerAirborneAttackState(PlayerMainController controllerScript, MainStateMachine stateMachine, PlayerMoves playerAttackAsset) : base(controllerScript, stateMachine, playerAttackAsset)
    {
        
            
    }
    public override void Enter()
    {
        base.Enter();
        controllerScript.attacksInTheAir -= 1;
    }
    public override void HandleUpdate()
    {
        base.HandleUpdate();

        if (controllerScript.isGrounded)
        {
            controllerScript.attacksInTheAir = 0;
            stateMachine.ChangeState(new PlayerStandingState(controllerScript, stateMachine));
            ParticleSystem landingDust = controllerScript.playerMainVFXManager.playerDustParticles.dustParticles["LandingDust"];
            landingDust.Play();
        }

    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        
        controllerScript.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * controllerScript.fallMultiplier * Time.deltaTime;

        controllerScript.playerRigidBody.velocity =
            new Vector2(controllerScript.playerRigidBody.velocity.x, controllerScript.playerRigidBody.velocity.y);

        
    }
    public override void Exit()
    {
        base.Exit();
        
    }
}
