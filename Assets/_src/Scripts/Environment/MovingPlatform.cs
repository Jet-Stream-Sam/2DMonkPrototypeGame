using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Rigidbody2D platformRigidbody;
    [SerializeField] private float speed;
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float stopTime = 0;
    private float stopTimer;
    private int movePointIndex = 0;
    private Vector2 originalPosition;

    private void Start()
    {
        originalPosition = platformRigidbody.position;
    }
    private void FixedUpdate()
    {
        bool isStopped = stopTimer > 0;

        bool isOnTheLastMovePoint = movePointIndex == -1;
        if (isOnTheLastMovePoint)
        {
            if (!isStopped)
            {
                platformRigidbody.position = Vector3.MoveTowards(platformRigidbody.position, originalPosition, speed * Time.fixedDeltaTime);
                if (platformRigidbody.position == originalPosition)
                {
                    movePointIndex = 0;
                    if(stopTime > 0)
                        StartCoroutine(StopMovement(stopTime));
                }
                return;
            }
            
          
        }

        if (!isStopped)
        {
            platformRigidbody.position = Vector3.MoveTowards(platformRigidbody.position, movePoints[movePointIndex].position, speed * Time.fixedDeltaTime);
            if (platformRigidbody.position == (Vector2)movePoints[movePointIndex].position)
            {
                movePointIndex++;
                if (movePointIndex >= movePoints.Length)
                    movePointIndex = -1;
                if (stopTime > 0)
                    StartCoroutine(StopMovement(stopTime));
            }
        }
        
 
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
}
