using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class EnemyAIBrain : SerializedMonoBehaviour
{
    [FoldoutGroup("Dependencies")]
    [SerializeField] protected Transform detectionTransform;
    [FoldoutGroup("Dependencies")]
    [SerializeField] protected EnemyMainController enemyController;
    [FoldoutGroup("Dependencies")]
    [OdinSerialize] public IMonoBehaviourState defaultState;
    [FoldoutGroup("Dependencies")]
    [InfoBox("This Dictionary needs specific behaviours to make the script work (Check script).")]
    [OdinSerialize] public Dictionary<string, IMonoBehaviourState> allStates;

    [TitleGroup("AI", Alignment = TitleAlignments.Centered)]
    
    [TabGroup("AI/Tabs", "Detection Settings")]
    [SerializeField] protected LayerMask detectionMask;
    [TabGroup("AI/Tabs", "Attack Settings")]
    [SerializeField] protected float attackCooldown = 0.5f;
    
    [TabGroup("AI/Tabs", "Debug")]
    [SerializeField] protected bool debugActivated = true;
    
    [ShowIf("debugActivated"), TabGroup("AI/Tabs", "Debug")]
    [ReadOnly] public GameObject focusedTarget;
    [ShowIf("debugActivated"), TabGroup("AI/Tabs", "Debug")]
    [SerializeField, ReadOnly] protected float attackCooldownTimer = 0;
    public MainMonoBehaviourStateMachine StateMachine { get; private set; }
    [ShowIf("debugActivated"), TabGroup("AI/Tabs", "Debug")]
    [ReadOnly] public string currentStateOutput;


    private void Start()
    {
        StateMachine = new MainMonoBehaviourStateMachine();

        StateMachine.onStateChanged += state => currentStateOutput = state;
        StateMachine.Init(defaultState);
        StateReset();
    }

    public void StateReset()
    {
        StateMachine.ChangeState(defaultState);
        attackCooldownTimer = attackCooldown;
    }
    public virtual void AIDetection()
    {
        
    }

    public void EnableAI()
    {
        enabled = true;
    }

    public void DisableAI()
    {
        StateReset();
        enabled = false;
    }
}
