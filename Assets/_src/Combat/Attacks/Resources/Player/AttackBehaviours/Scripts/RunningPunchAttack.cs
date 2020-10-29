using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningPunchAttack : MonoBehaviour, IAttackBehaviour
{
    private PlayerMainController controllerScript;
    public void Init(PlayerMainController controllerScript)
    {
        this.controllerScript = controllerScript;
    }
    public void OnAttackEnter()
    {
        Debug.Log("Back in the game");
    }

    public void OnAttackUpdate()
    {

    }

    public void OnAttackFixedUpdate()
    {

    }

    public void OnAttackExit()
    {

    }
}
