using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();

    void HandleUpdate();

    void HandleFixedUpdate();
    void Exit();
}
