using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCharge : MonoBehaviour, IMoveBehaviour
{
    private EnemyMainController controllerScript;
    private EnemyMoves attackAsset;
    private FlyingEnemyTargetedAttackState attackState;
    [SerializeField] private float dashPower = 7;
    [SerializeField] private float easingRate = 0.8f;
    private Vector2 easingMovement;

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
        if (state is FlyingEnemyTargetedAttackState attackState)
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
            Vector2.Lerp(easingMovement,
            Vector2.zero,
            easingRate);

        controllerScript.enemyRigidBody.velocity = easingMovement;

        if (Mathf.Abs(controllerScript.enemyCollider.transform.position.magnitude - lastImagePos) > allowedDistanceBtwImages)
        {
            vfxPool.GetFromPool();
            lastImagePos = controllerScript.enemyCollider.transform.position.magnitude;
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


        controllerScript.SetMovement(attackState.directionToFollow);

        controllerScript.enemyRigidBody.velocity = attackState.directionToFollow * dashPower;

        easingMovement = controllerScript.enemyRigidBody.velocity;

        vfxPool = controllerScript.enemyVFXManager.afterImageEffectPool;
        vfxPool.UpdatePool(afterImageEffect);
        vfxPool.GetFromPool();

        lastImagePos = controllerScript.enemyCollider.transform.position.magnitude;
        attackHasTriggered = true;
    }
}
