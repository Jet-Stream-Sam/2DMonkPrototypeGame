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

        controllerScript.isReversed = !controllerScript.Flip(controllerScript.enemySpriteTransform, controllerScript.MovementX);
    }

    public virtual void HandleFixedUpdate()
    {

    }

    public virtual void Exit()
    {

    }

    protected float ClampMovement(float value)
    {
        if (value < 0.01f && value > -0.01f)
        {
            value = 0;
        }
        if (value > 0.99f)
        {
            value = 1;
        }
        if (value < -0.99f)
        {
            value = -1;
        }
        return value;
    }
}
