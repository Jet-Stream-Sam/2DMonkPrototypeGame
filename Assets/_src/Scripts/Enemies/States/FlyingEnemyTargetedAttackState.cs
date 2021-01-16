using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class FlyingEnemyTargetedAttackState : EnemyState
{
    private MainVFXManager VFXManager;
    protected CancellationTokenSource tokenSource;
    private Vector3 initialEnemyScale;
    private bool lockAsyncMethod;
    public Vector2 directionToFollow { get; private set; }
    public Transform focusedTargetTransform { get; private set; }
    private Transform enemyTransform;
    private EnemyMoves attackAsset;
    private string animationToPlay;
    private CollectionSounds moveSound;
    private CollectionSounds crySound;
    private bool lockVelocity;
    private bool lockSideSwitch;
    private float attackDuration;
    private float attackTimer;
    private HitProperties hitProperties;
    protected IMoveBehaviour attackBehaviour;
    protected Moves.MoveType moveType;
    protected bool attackAndProjectile;
    protected bool wasFlipped;
    public FlyingEnemyTargetedAttackState(EnemyMainController controllerScript, MainStateMachine stateMachine,
        EnemyMoves enemyAttackAsset, Transform target) : base(controllerScript, stateMachine)
    {
        attackAsset = enemyAttackAsset;
        animationToPlay = enemyAttackAsset.animationClip.name;
        moveSound = enemyAttackAsset.moveSoundEffect;
        crySound = enemyAttackAsset.crySoundEffect;
        lockVelocity = enemyAttackAsset.lockVelocity;
        lockSideSwitch = enemyAttackAsset.lockSideSwitch;
        hitProperties = enemyAttackAsset.hitProperties;
        attackDuration = enemyAttackAsset.animationClip.length;
        focusedTargetTransform = target;
        moveType = enemyAttackAsset.moveType;
        if(moveType == Moves.MoveType.Projectile)
        {
            attackAndProjectile = enemyAttackAsset.attackAndProjectile;
            controllerScript.AnimationEventWasCalled += ShootProjectile;
        }

        
        if (enemyAttackAsset.moveBehaviour is IMoveBehaviour attack)
        {
            if (attack is MonoBehaviour attackComponent)
            {
                attackBehaviour = (IMoveBehaviour)UnityEngine.Object.Instantiate(attackComponent, controllerScript.transform);
            }
            
        }
    }

    public override void Enter()
    {
        
        base.Enter();

        VFXManager = controllerScript.enemyVFXManager;
        attackBehaviour?.Init(controllerScript, attackAsset, this);

        enemyTransform = controllerScript.enemySpriteTransform;
        directionToFollow = ((Vector2)focusedTargetTransform.position - (Vector2)enemyTransform.position).normalized;

        controllerScript.spriteFlip.Flip(directionToFollow.x);
        initialEnemyScale = enemyTransform.localScale;

        controllerScript.enemyAnimationsScript.ChangeAnimationState(animationToPlay, false);

        if (lockVelocity)
            LockVelocity();

        SoundManager soundManager = SoundManager.Instance;

        if(moveSound != null)
            moveSound.PlaySound(soundManager, enemyTransform.position);

        if (crySound != null)
            crySound.PlaySound(soundManager, enemyTransform.position);

        controllerScript.hitBoxCheck.HitProperties = hitProperties;

        if (moveType == Moves.MoveType.Projectile && !attackAndProjectile)
            hitProperties = null;
        if (!lockAsyncMethod)
            AttackLoop();

        attackBehaviour?.OnMoveEnter();
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();

        if (lockSideSwitch)
            LockSideSwitch(initialEnemyScale);
        else
        {
            if(focusedTargetTransform != null)
            {
                directionToFollow = ((Vector2)focusedTargetTransform.position - (Vector2)enemyTransform.position).normalized;
                controllerScript.spriteFlip.Flip(directionToFollow.x);
                
            }
            
        }
            
        attackBehaviour?.OnMoveUpdate();
    }

    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
        attackBehaviour?.OnMoveFixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();

        if(moveType == Moves.MoveType.Projectile)
        {
            controllerScript.AnimationEventWasCalled -= ShootProjectile;
        }
        tokenSource.Cancel();
        controllerScript.hitBoxCheck.ResetProperties();
        controllerScript.AIBrain.StateReset();
        attackBehaviour?.OnMoveExit();

        if(attackBehaviour != null)
            UnityEngine.Object.Destroy(((MonoBehaviour)attackBehaviour).gameObject);
    }

    private async void AttackLoop()
    {
        tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        lockAsyncMethod = true;

        attackTimer = attackDuration;

        while (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            await Task.Yield();
        }

        if (token.IsCancellationRequested)
            return;

        stateMachine.ChangeState(new FlyingEnemyIdleState(controllerScript, stateMachine));
    }

    private void LockVelocity()
    {
        controllerScript.enemyRigidBody.velocity = new Vector2(0, 0);
    }
    private void LockSideSwitch(Vector3 initialScale)
    {
        controllerScript.enemySpriteTransform.localScale = initialScale;
    }

    private void ShootProjectile(ScriptableObject obj)
    {
        if (!(obj is ProjectileTriggerEvent projEvent))
            return;

        GameObject instantiatedObj = Object.Instantiate(projEvent.fireballPrefab, controllerScript.enemyProjectileTransform.position, Quaternion.identity, VFXManager.transform);
        FireballBehaviour fireball = instantiatedObj.GetComponent<FireballBehaviour>();
        ProjectileHitCheck projectileHitBox = instantiatedObj.GetComponent<ProjectileHitCheck>();
        fireball.target = focusedTargetTransform;
        projectileHitBox.hitInstanceException = controllerScript.GetComponentInChildren<EnemyMainTrigger>();
        
    }

}
