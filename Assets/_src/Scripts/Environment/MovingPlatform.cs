using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Rigidbody2D platformRigidbody;
    [SerializeField] private float platformSpeed;
    [SerializeField] private PlatformFinishRoute platformFinishRoute;
    [SerializeField] private bool platformWaitsForPlayer;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float pointDetectionRadius;
    private PlayerMainController playerController;
    private Rigidbody2D attachedRb;
    
    [SerializeField] private float stopTime = 0;
    private float stopTimer;

    private int movePointIndex = 0;
    private bool reversed = false;
    private bool isStopped;


    public enum PlatformFinishRoute
    {
        BackToFirstPoint,
        Reverse
    }

    private void Start()
    {
        if (!platformWaitsForPlayer)
            MovementStart();
    }

    private void MovementStart()
    {
        isStopped = false;
    }

    private void MovementStop()
    {
        isStopped = true;
    }
    private void FixedUpdate()
    {
        bool isWaiting = stopTimer > 0;

        bool isOnTheLastMovePoint = movePointIndex == movePoints.Length;

        if (isWaiting || isStopped)
            return;

        if (!isOnTheLastMovePoint)
        {
            bool canPlatformDepart = !(movePointIndex == 0 && platformWaitsForPlayer && !attachedRb && !reversed);
            if (!canPlatformDepart)
                return;

            MovePlatform(movePoints[movePointIndex].position, platformSpeed, AdvancePointIndex);

            void AdvancePointIndex()
            {
                ChooseNextPoint();
                if (stopTime > 0)
                    StartCoroutine(StopMovement(stopTime));
            }
        }
        else
        {
            if(platformFinishRoute == PlatformFinishRoute.BackToFirstPoint)
            {
                reversed = true;
                movePointIndex = 0;
                MovePlatform(movePoints[movePointIndex].position, platformSpeed, ResetPointIndex);
            }    
            else if(platformFinishRoute == PlatformFinishRoute.Reverse)
            {
                reversed = true;
                movePointIndex -= 2;
                MovePlatform(movePoints[movePointIndex].position, platformSpeed, ReversePointIndex);
            }
                
            void ResetPointIndex()
            {
                ChooseNextPoint();
                if (stopTime > 0)
                    StartCoroutine(StopMovement(stopTime));
            }

            void ReversePointIndex()
            {
                ChooseNextPoint();
                if (stopTime > 0)
                    StartCoroutine(StopMovement(stopTime));
            }
        }

        if (attachedRb != null)
            AttachRigidBodyToPlatform(attachedRb);
    }
    private void AttachRigidBodyToPlatform(Rigidbody2D rb)
    {
        rb.velocity = new Vector2(rb.velocity.x + platformRigidbody.velocity.x, platformRigidbody.velocity.y);
    }

    private void MovePlatform(Vector2 point, float speed, Action onFinish) 
    {
        Vector2 movingDirection = (point - platformRigidbody.position).normalized;
        platformRigidbody.velocity = new Vector2(movingDirection.x * speed, movingDirection.y * speed);

        bool distanceCheck = Vector2.Distance(point, platformRigidbody.position) < pointDetectionRadius;
        if (distanceCheck)
        {
            platformRigidbody.velocity = Vector2.zero;
            onFinish?.Invoke();
        }
    }

    private void ChooseNextPoint()
    {
        if (movePointIndex == 0)
        {
            reversed = false;
            if (platformWaitsForPlayer && !attachedRb)
            {
                MovementStop();
                return;
            }
                
        }
            
        if (!reversed)
            movePointIndex++;
        else
            movePointIndex--;
    }
    IEnumerator StopMovement(float seconds)
    {
        
        stopTimer = seconds;
        while (stopTimer > 0)
        {
            stopTimer -= Time.deltaTime;
            yield return null;
        }
        stopTimer = 0;

    }

    private void DisconnectRigidBody()
    {
        attachedRb = null;

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out PlayerMainTrigger player))
            return;

        if (platformWaitsForPlayer && isStopped)
            MovementStart();
        attachedRb = col.attachedRigidbody;
        playerController = player.playerController;
        playerController.hasPerformedJump += DisconnectRigidBody;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.TryGetComponent(out PlayerMainTrigger player))
            return;

        DisconnectRigidBody();
        playerController.hasPerformedJump -= DisconnectRigidBody;

    }

    private void OnDestroy()
    {
        playerController.hasPerformedJump -= DisconnectRigidBody;
    }
}
