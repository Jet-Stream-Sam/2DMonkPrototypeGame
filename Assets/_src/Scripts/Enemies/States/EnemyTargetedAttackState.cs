using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System.CodeDom;

public class EnemyTargetedAttackState : EnemyState
{
    private GlobalVFXManager VFXManager;
    protected CancellationTokenSource tokenSource;
    private Vector3 initialEnemyScale;
    private bool lockAsyncMethod;
    public Vector2 directionToFollow { get; private set; }
    public Transform focusedTargetTransform { get; private set; }
    private Transform enemyTransform;
    private EnemyMoves attackAsset;
    private string animationToPlay;
    private CollectionSounds audioClip;
    private bool lockVelocity;
    private bool lockSideSwitch;
    private float attackDuration;
    private float attackTimer;
    private HitProperties hitProperties;
    protected IMoveBehaviour attackBehaviour;
    protected Moves.MoveType moveType;
    protected bool attackAndProjectile;
    protected bool wasFlipped;
    public EnemyTargetedAttackState(EnemyMainController controllerScript, MainStateMachine stateMachine,
        EnemyMoves enemyAttackAsset, Transform target) : base(controllerScript, stateMachine)
    {
        attackAsset = enemyAttackAsset;
        animationToPlay = enemyAttackAsset.animationClip.name;
        audioClip = enemyAttackAsset.moveSoundEffect;
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
            attackBehaviour = attack;
        }
    }

    public override void Enter()
    {
        
        base.Enter();

        VFXManager = GlobalVFXManager.Instance;
        attackBehaviour?.Init(controllerScript, attackAsset, this);

        enemyTransform = controllerScript.enemySpriteTransform;
        directionToFollow = new Vector2(focusedTargetTransform.position.x - controllerScript.enemySpriteTransform.position.x, 0).normalized;

        controllerScript.spriteFlip.Flip(directionToFollow.x);
        initialEnemyScale = enemyTransform.localScale;

        controllerScript.enemyAnimationsScript.ChangeAnimationState(animationToPlay, false);

        if (lockVelocity)
            LockVelocity();

        SoundManager soundManager = SoundManager.Instance;

        if(audioClip != null)
            audioClip.PlaySound(soundManager);

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

        if (!controllerScript.isGrounded)
        {
            stateMachine.ChangeState(new EnemyFallingState(controllerScript, stateMachine));
        }
        if (lockSideSwitch)
            LockSideSwitch(initialEnemyScale);
        else
        {
            directionToFollow = new Vector2(focusedTargetTransform.position.x - controllerScript.enemySpriteTransform.position.x, 0).normalized;
            controllerScript.spriteFlip.Flip(directionToFollow.x);
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

        EndMoveAtState();
    }

    private void EndMoveAtState()
    {
        if (controllerScript == null)
            return;
        if (controllerScript.isGrounded)
            stateMachine.ChangeState(new EnemyStandingState(controllerScript, stateMachine));
        else
            stateMachine.ChangeState(new EnemyFallingState(controllerScript, stateMachine));
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
        projectileHitBox.hitInstanceException = controllerScript;
        
    }
}
