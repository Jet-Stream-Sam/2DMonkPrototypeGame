using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttackBehaviour : MonoBehaviour, IMonoBehaviourState
{
    [SerializeField] private EnemyAIBrain enemyAI;
    [SerializeField] private EnemyMainController enemyController;
    [SerializeField] private EnemyMoveList moveList;
    private EnemyMoves selectedAttack;
    private GameObject player;
    private Transform playerTransform;


    private void OnEnable()
    {
        if (enemyController.currentStateOutput != "EnemyStandingState")
            return;

        player = enemyAI.focusedTarget;
        playerTransform = player.transform;

        selectedAttack = moveList.enemyMoveList[Random.Range(0, moveList.enemyMoveList.Length)];
        enemyController.StateMachine.ChangeState(new EnemyTargetedAttackState(enemyController, enemyController.StateMachine,
            selectedAttack, playerTransform));
    }

}
