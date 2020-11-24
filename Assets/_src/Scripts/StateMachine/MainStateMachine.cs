using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStateMachine
{
    public IState CurrentState { get; private set; }

    public Action onStateChanged;
    public void Init(IState initalState)
    {
        CurrentState = initalState;
        CurrentState.Enter();
    }

    public void ChangeState(IState newState)
    {
        CurrentState.Exit();
        onStateChanged?.Invoke();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
