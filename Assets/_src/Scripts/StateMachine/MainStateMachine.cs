using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStateMachine
{
    public IState CurrentState { get; private set; }
    

    public void Init(IState initalState)
    {
        CurrentState = initalState;
        CurrentState.Enter();
    }

    public void ChangeState(IState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
