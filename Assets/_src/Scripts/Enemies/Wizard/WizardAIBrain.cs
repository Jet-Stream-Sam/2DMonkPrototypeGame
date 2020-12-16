using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class WizardAIBrain : EnemyAIBrain
{
    [TabGroup("AI/Tabs", "Detection Settings")]
    [SerializeField] private float detectAndFollowRange;
    [TabGroup("AI/Tabs", "Detection Settings")]
    [SerializeField] private float attackRange;

    [ShowIf("debugActivated"), TabGroup("AI/Tabs", "Debug")]
    [SerializeField, ReadOnly] private bool hasDetectedTarget;
    [ShowIf("debugActivated"), TabGroup("AI/Tabs", "Debug")]
    [SerializeField, ReadOnly] private bool isGoingToAttackTarget;

    private void Update()
    {
        AIDetection();
    }
    public override void AIDetection()
    {
        base.AIDetection();

        if (enemyController.currentStateOutput != "EnemyStandingState")
            return;

        hasDetectedTarget = Physics2D.OverlapCircle(detectionTransform.position, detectAndFollowRange, detectionMask);
        isGoingToAttackTarget = Physics2D.OverlapCircle(detectionTransform.position, attackRange, detectionMask);

        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer < 0)
                attackCooldownTimer = 0;
        }

        if (isGoingToAttackTarget && attackCooldownTimer == 0)
        {
            Collider2D target = Physics2D.OverlapCircle(detectionTransform.position, attackRange, detectionMask);
            focusedTarget = target.gameObject;
            StateMachine.ChangeState(allStates["AttackBehaviour"]);
        }
        else if (hasDetectedTarget)
        {
            Collider2D target = Physics2D.OverlapCircle(detectionTransform.position, detectAndFollowRange, detectionMask);
            focusedTarget = target.gameObject;
            StateMachine.ChangeState(allStates["FollowBehaviour"]);
        }
        else
        {
            focusedTarget = null;
            StateMachine.ChangeState(allStates["WanderBehaviour"]);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (debugActivated)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(detectionTransform.position, detectAndFollowRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(detectionTransform.position, attackRange);
        }

    }
}
