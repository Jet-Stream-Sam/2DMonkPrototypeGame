using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyState : IState
{
    protected EnemyMainController controllerScript;
    protected MainStateMachine stateMachine;
    public bool OverridingState { get; set; }
    public FlyingEnemyState(EnemyMainController controllerScript, MainStateMachine stateMachine)
    {
        this.controllerScript = controllerScript;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter()
    {
        OverridingState = false;
    }

    public virtual void HandleUpdate()
    {
        controllerScript.isGrounded = Physics2D.OverlapCircle(
            controllerScript.groundCheck.position,
            controllerScript.groundCheckRadius,
            controllerScript.groundMask);
    }

    public virtual void HandleFixedUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
