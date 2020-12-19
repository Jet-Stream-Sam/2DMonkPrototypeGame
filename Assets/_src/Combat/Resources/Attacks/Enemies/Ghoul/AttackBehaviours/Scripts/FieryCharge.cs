using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieryCharge : MonoBehaviour, IMoveBehaviour
{
    private EnemyMainController controllerScript;
    private EnemyMoves attackAsset;
    private EnemyTargetedAttackState attackState;
    [SerializeField] private float dashPower = 7;
    [SerializeField] private float easingRate = 0.8f;
    private float easingMovement;

    private AfterImageEffectPool vfxPool;
    [SerializeField] private GameObject afterImageEffect;
    [SerializeField] private float allowedDistanceBtwImages = 0.8f;
    private float lastImagePos;

    private bool attackHasTriggered;

    public void Init(IEntityController controllerScript, Moves attackAsset, IState state)
    {
        if (controllerScript is EnemyMainController controller)
        {
            this.controllerScript = controller;
        }
        if (attackAsset is EnemyMoves playerMoves)
        {
            this.attackAsset = playerMoves;
        }
        if(state is EnemyTargetedAttackState attackState)
        {
            this.attackState = attackState;
        }
    }
    public void OnMoveEnter()
    {
        controllerScript.AnimationEventWasCalled += TriggerAttack;
    }

    public void OnMoveUpdate()
    {



    }

    public void OnMoveFixedUpdate()
    {
        if (!attackHasTriggered)
            return;

        easingMovement =
            Mathf.Lerp(easingMovement,
            0,
            easingRate);

        float tempSpeed = easingMovement;

        controllerScript.enemyRigidBody.velocity =
            new Vector2(tempSpeed, controllerScript.enemyRigidBody.velocity.y);

        if (Mathf.Abs(controllerScript.enemyCollider.transform.position.x - lastImagePos) > allowedDistanceBtwImages)
        {
            vfxPool.GetFromPool();
            lastImagePos = controllerScript.enemyCollider.transform.position.x;
        }
    }

    public void OnMoveExit()
    {
        attackHasTriggered = false;
        controllerScript.AnimationEventWasCalled -= TriggerAttack;
    }

    private void TriggerAttack(ScriptableObject obj)
    {
        if (!(obj is AttackTriggerEvent trigger))
            return;

        
        controllerScript.enemyRigidBody.velocity =
            new Vector2(attackState.directionToFollow.x * dashPower,
            controllerScript.enemyRigidBody.velocity.y);


        easingMovement = controllerScript.enemyRigidBody.velocity.x;

        vfxPool = controllerScript.enemyVFXManager.afterImageEffectPool;
        vfxPool.UpdatePool(afterImageEffect);
        vfxPool.GetFromPool();

        lastImagePos = controllerScript.enemyCollider.transform.position.x;
        attackHasTriggered = true;
    }
}
