using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelAttackBehaviour : MonoBehaviour, IMonoBehaviourState
{
    [SerializeField] private EnemyAIBrain enemyAI;
    [SerializeField] private FlyingEnemyMainController enemyController;
    [SerializeField] private EnemyMoveList moveList;
    private EnemyMoves selectedAttack;
    private GameObject player;
    private Transform playerTransform;


    private void OnEnable()
    {

        player = enemyAI.focusedTarget;
        playerTransform = player.transform;

        selectedAttack = moveList.enemyMoveList[Random.Range(0, moveList.enemyMoveList.Length)];
        enemyController.StateMachine.ChangeState(new FlyingEnemyTargetedAttackState(enemyController, enemyController.StateMachine,
            selectedAttack, playerTransform));
    }
}
