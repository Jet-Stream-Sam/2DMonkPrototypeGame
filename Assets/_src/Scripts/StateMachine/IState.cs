using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    bool OverridingState { get; set; }
    void Enter();

    void HandleUpdate();

    void HandleFixedUpdate();
    void Exit();
}
