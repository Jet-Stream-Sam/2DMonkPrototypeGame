using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class EnemyAIBrain : SerializedMonoBehaviour
{
    [FoldoutGroup("Dependencies")]
    [SerializeField] private Transform detectionTransform;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private EnemyMainController enemyController;
    [FoldoutGroup("Dependencies")]
    [OdinSerialize] public IMonoBehaviourState defaultState;
    [FoldoutGroup("Dependencies")]
    [InfoBox("This Dictionary needs specific behaviours to make the script work (Check script).")]
    [OdinSerialize] public Dictionary<string, IMonoBehaviourState> allStates;

    [TitleGroup("AI", Alignment = TitleAlignments.Centered)]
    [TabGroup("AI/Tabs", "Detection Settings")]
    [SerializeField] private float detectAndFollowRange;
    [TabGroup("AI/Tabs", "Detection Settings")]
    [SerializeField] private float attackRange;
    
    [TabGroup("AI/Tabs", "Detection Settings")]
    [SerializeField] private LayerMask detectionMask;
    [TabGroup("AI/Tabs", "Attack Settings")]
    [SerializeField] private float attackCooldown = 0.5f;
    
    [TabGroup("AI/Tabs", "Debug")]
    [SerializeField] private bool debugActivated = true;
    [ShowIf("debugActivated"), TabGroup("AI/Tabs", "Debug")]
    [SerializeField, ReadOnly] private bool hasDetectedTarget;
    [ShowIf("debugActivated"), TabGroup("AI/Tabs", "Debug")]
    [SerializeField, ReadOnly] private bool isGoingToAttackTarget;
    [ShowIf("debugActivated"), TabGroup("AI/Tabs", "Debug")]
    [ReadOnly] public GameObject focusedTarget;
    [ShowIf("debugActivated"), TabGroup("AI/Tabs", "Debug")]
    [SerializeField, ReadOnly] private float attackCooldownTimer = 0;
    public MainMonoBehaviourStateMachine StateMachine { get; private set; }
    [ShowIf("debugActivated"), TabGroup("AI/Tabs", "Debug")]
    [ReadOnly] public string currentStateOutput;


    private void Start()
    {
        StateMachine = new MainMonoBehaviourStateMachine();

        StateMachine.onStateChanged += state => currentStateOutput = state;
        StateMachine.Init(defaultState);
    }
    private void Update()
    {
        hasDetectedTarget = Physics2D.OverlapCircle(detectionTransform.position, detectAndFollowRange, detectionMask);
        isGoingToAttackTarget = Physics2D.OverlapCircle(detectionTransform.position, attackRange, detectionMask);

        if(attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer < 0)
                attackCooldownTimer = 0;
        }
        AIDetection();
        
        
    }

    public void StateReset()
    {
        StateMachine.ChangeState(defaultState);
        attackCooldownTimer = attackCooldown;
    }
    public virtual void AIDetection()
    {
        if (enemyController.currentStateOutput != "EnemyStandingState")
            return;

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
