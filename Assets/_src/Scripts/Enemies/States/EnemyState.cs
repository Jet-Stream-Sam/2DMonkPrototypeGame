using UnityEngine;
using System.Collections;

public class EnemyState : IState
{
    protected EnemyMainController controllerScript;
    protected MainStateMachine stateMachine;
    public bool OverridingState { get; set; }
    public EnemyState(EnemyMainController controllerScript, MainStateMachine stateMachine)
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
