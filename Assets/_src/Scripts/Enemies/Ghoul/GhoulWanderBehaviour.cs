using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulWanderBehaviour : MonoBehaviour, IMonoBehaviourState
{
    [SerializeField] private EnemyMainController enemyController;
    private Vector2 directionToFollow;
    private Transform groundDetectionLeft;
    private Transform groundDetectionRight;
    private Transform wallDetectionLeft;
    private Transform wallDetectionRight;
    private float groundDetectionLRadius;
    private float groundDetectionRRadius;
    private float wallDetectionLRadius;
    private float wallDetectionRRadius;
    private LayerMask groundLayerMask;
    private bool isCheckingGroundDetectionOnTheLeft;
    private bool isCheckingGroundDetectionOnTheRight;
    private bool isCheckingWallDetectionOnTheLeft;
    private bool isCheckingWallDetectionOnTheRight;
    private void Start()
    {
        groundDetectionLeft = enemyController.groundDetectionLeft;
        groundDetectionRight = enemyController.groundDetectionRight;
        wallDetectionLeft = enemyController.wallDetectionLeft;
        wallDetectionRight = enemyController.wallDetectionRight;
        wallDetectionLRadius = enemyController.wallDetectionLRadius;
        wallDetectionRRadius = enemyController.wallDetectionRRadius;
        groundDetectionLRadius = enemyController.groundDetectionLRadius;
        groundDetectionRRadius = enemyController.groundDetectionRRadius;
        groundLayerMask = enemyController.groundMask;
    }

    private void OnEnable()
    {
        directionToFollow = new Vector2(Random.Range(-1, 2), 0).normalized;

        if (directionToFollow.x == 0)
        {
            directionToFollow = new Vector2(1, 0);
        }
    }

    public void FixedUpdate()
    {
        if (enemyController.currentStateOutput != "EnemyStandingState")
            return;

        isCheckingGroundDetectionOnTheLeft = Physics2D.OverlapCircle(
            groundDetectionLeft.position,
            groundDetectionLRadius,
            groundLayerMask);

        isCheckingGroundDetectionOnTheRight = Physics2D.OverlapCircle(
            groundDetectionRight.position,
            groundDetectionRRadius,
            groundLayerMask);

        isCheckingWallDetectionOnTheLeft = Physics2D.OverlapCircle(
            wallDetectionLeft.position,
            wallDetectionLRadius,
            groundLayerMask);

        isCheckingWallDetectionOnTheRight = Physics2D.OverlapCircle(
            wallDetectionRight.position,
            wallDetectionRRadius,
            groundLayerMask);

        
        if ((!isCheckingGroundDetectionOnTheRight ||
            isCheckingWallDetectionOnTheRight) && enemyController.isReversed)
        {
            directionToFollow = new Vector2(1, 0);
        }
        if ((!isCheckingGroundDetectionOnTheRight ||
                    isCheckingWallDetectionOnTheRight) && !enemyController.isReversed)
        {
            directionToFollow = new Vector2(-1, 0);
        }

        if (!isCheckingGroundDetectionOnTheLeft && !isCheckingGroundDetectionOnTheRight ||
            isCheckingWallDetectionOnTheLeft && isCheckingWallDetectionOnTheRight)
        {
            enemyController.SetMovement(Vector2.zero);
            return;
        }

        enemyController.SetMovement(directionToFollow);
    }
}
