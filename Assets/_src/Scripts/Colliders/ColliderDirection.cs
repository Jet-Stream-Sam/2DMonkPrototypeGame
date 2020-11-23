using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDirection : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Vector2 AttackDirection { get; private set; }

    private void Update()
    {
        AttackDirection = (endPoint.position - startPoint.position).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPoint.position, endPoint.position);
    }
}
