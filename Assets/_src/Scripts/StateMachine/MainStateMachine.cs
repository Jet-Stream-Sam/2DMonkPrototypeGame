using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStateMachine
{
    public IState CurrentState { get; private set; }

    public Action<string> onStateChanged;
    public void Init(IState initalState)
    {
        CurrentState = initalState;
        CurrentState.Enter();
    }

    public void ChangeState(IState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        onStateChanged?.Invoke(newState.ToString());
        CurrentState.Enter();
    }
}
