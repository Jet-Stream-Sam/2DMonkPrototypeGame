using UnityEngine;
using System;
using System.Collections;

public class MainMonoBehaviourStateMachine : MonoBehaviour
{
    public IMonoBehaviourState CurrentState { get; private set; }

    public Action<string> onStateChanged;
    public void Init(IMonoBehaviourState initialState)
    {
        CurrentState = initialState;

        onStateChanged?.Invoke(initialState.ToString());
        Enter(CurrentState);
    }

    public void ChangeState(IMonoBehaviourState newState)
    {
        if (newState == CurrentState)
            return;

        Exit(CurrentState);
        CurrentState = newState;
        onStateChanged?.Invoke(newState.ToString());
        Enter(CurrentState);
    }

    void Enter(IMonoBehaviourState state)
    {
        MonoBehaviour mb = state as MonoBehaviour;
        if (mb != null)
        {
            mb.enabled = true;
        }
    }

    void Exit(IMonoBehaviourState state)
    {
        MonoBehaviour mb = state as MonoBehaviour;
        if (mb != null)
        {
            mb.enabled = false;
        }
    }
}
