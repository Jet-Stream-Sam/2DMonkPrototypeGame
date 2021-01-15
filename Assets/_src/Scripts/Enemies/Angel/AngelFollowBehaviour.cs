using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelFollowBehaviour : MonoBehaviour, IMonoBehaviourState
{
    [SerializeField] private EnemyAIBrain enemyAI;
    [SerializeField] private FlyingEnemyMainController enemyController;
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
        enemyRigidBody.velocity = new Vector2(0, 0);
    }
    private void FixedUpdate()
    {
        if (player != null)
        {
            directionToFollow = (playerTransform.position - enemyController.enemySpriteTransform.position).normalized;

 
            if (Time.deltaTime > 0)
                enemyRigidBody.velocity = new Vector2(directionToFollow.x * enemySpeed, directionToFollow.y * enemySpeed);


            enemyController.spriteFlip.Flip(directionToFollow.x);



        }
    }
}
