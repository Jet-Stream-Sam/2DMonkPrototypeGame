using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardFollowBehaviour : MonoBehaviour, IMonoBehaviourState
{
    [SerializeField] private EnemyAIBrain enemyAI;
    [SerializeField] private EnemyMainController enemyController;
    private Rigidbody2D enemyRigidBody;
    private GameObject player;
    private Transform playerTransform;
    private Vector2 directionToFollow;
    private float enemySpeed;

    private void Start()
    {
        enemySpeed = enemyController.enemySpeed;
        player = enemyAI.focusedTarget;
        playerTransform = player.transform;
    }

    private void OnEnable()
    {
        enemyRigidBody = enemyController.enemyRigidBody;
        enemyRigidBody.velocity = new Vector2(0, enemyRigidBody.velocity.y);
    }
    private void FixedUpdate()
    {
        if (enemyController.currentStateOutput != "EnemyStandingState")
            return;

        if (player != null)
        {
            directionToFollow = new Vector2(playerTransform.position.x - enemyController.enemySpriteTransform.position.x, 0).normalized;

            if(directionToFollow.x == 0)
            {
                directionToFollow = new Vector2(1, 0);
            }

            if (Time.deltaTime > 0)
                enemyController.SetMovement(directionToFollow);
                 
        }
    }

}
