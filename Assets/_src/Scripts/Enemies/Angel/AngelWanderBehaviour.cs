using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelWanderBehaviour : MonoBehaviour, IMonoBehaviourState
{
    [SerializeField] private FlyingEnemyMainController enemyController;

    public void FixedUpdate()
    {
        if (enemyController.currentStateOutput != "FlyingEnemyIdleState")
            return;
        enemyController.enemyRigidBody.velocity = Vector2.zero;
    }
}
