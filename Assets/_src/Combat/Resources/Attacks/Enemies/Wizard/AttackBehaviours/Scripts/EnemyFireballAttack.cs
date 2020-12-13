using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireballAttack : MonoBehaviour, IMoveBehaviour
{
    private EnemyMainController controllerScript;
    private EnemyMoves attackAsset;
    private EnemyTargetedAttackState attackState;
    private Transform targetTransform;
    public void Init(IEntityController controllerScript, Moves attackAsset, IState state)
    {
        if (controllerScript is EnemyMainController controller)
        {
            this.controllerScript = controller;
        }

        if(state is EnemyTargetedAttackState attackState)
        {
            this.attackState = attackState;
        }

    }

    public void OnMoveEnter()
    {
        targetTransform = attackState.focusedTargetTransform;
    }

    public void OnMoveUpdate()
    {

    }

    public void OnMoveFixedUpdate()
    {

    }

    public void OnMoveExit()
    {

    }
}
