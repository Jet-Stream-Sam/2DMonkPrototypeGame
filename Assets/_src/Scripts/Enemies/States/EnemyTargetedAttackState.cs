using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class EnemyTargetedAttackState : EnemyState
{
    protected CancellationTokenSource tokenSource;
    private Vector3 initialEnemyScale;
    private bool lockAsyncMethod;
    private Vector2 directionToFollow;

    public Transform focusedTargetTransform { get; private set; }
    private Transform enemyTransform;
    private EnemyMoves attackAsset;
    private string animationToPlay;
    private string audioClipName;
    private bool lockVelocity;
    private bool lockSideSwitch;
    private float attackDuration;
    private float attackTimer;
    private HitProperties hitProperties;
    protected IMoveBehaviour attackBehaviour;
    protected Moves.MoveType moveType;
    protected bool attackAndProjectile;
    protected HitProperties projectileHitProperties;
    public EnemyTargetedAttackState(EnemyMainController controllerScript, MainStateMachine stateMachine,
        EnemyMoves enemyAttackAsset, Transform target) : base(controllerScript, stateMachine)
    {
        attackAsset = enemyAttackAsset;
        animationToPlay = enemyAttackAsset.animationClip.name;
        if (enemyAttackAsset.moveSoundEffect != null)
            audioClipName = enemyAttackAsset.moveSoundEffect.name;
        lockVelocity = enemyAttackAsset.lockVelocity;
        lockSideSwitch = enemyAttackAsset.lockSideSwitch;
        hitProperties = enemyAttackAsset.hitProperties;
        attackDuration = enemyAttackAsset.animationClip.length;
        focusedTargetTransform = target;
        moveType = enemyAttackAsset.moveType;
        if(moveType == Moves.MoveType.Projectile)
        {
            attackAndProjectile = enemyAttackAsset.attackAndProjectile;
            projectileHitProperties = enemyAttackAsset.projectileHitProperties;

            controllerScript.hasShotAProjectile += ShootProjectile;
        }

        if (enemyAttackAsset.moveBehaviour is IMoveBehaviour attack)
        {
            attackBehaviour = attack;
        }
    }

    public override void Enter()
    {
        
        base.Enter();
        
        attackBehaviour?.Init(controllerScript, attackAsset, this);

        enemyTransform = controllerScript.enemySpriteTransform;
        initialEnemyScale = enemyTransform.localScale;
        

        controllerScript.enemyAnimationsScript.ChangeAnimationState(animationToPlay);

        if (lockVelocity)
            LockVelocity();

        SoundManager soundManager = SoundManager.Instance;
        if (audioClipName != null)
            soundManager.PlayOneShotSFX(audioClipName);

        controllerScript.hitBoxCheck.HitProperties =
            new HitProperties(hitProperties);

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
            directionToFollow = new Vector2(focusedTargetTransform.position.x - enemyTransform.position.x, 0).normalized;

            switch (directionToFollow.x)
            {
                case -1:
                    controllerScript.enemySpriteRenderer.flipX = false;
                    break;
                case 1:
                    controllerScript.enemySpriteRenderer.flipX = true;
                    break;
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
            controllerScript.hasShotAProjectile -= ShootProjectile;
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

    private void ShootProjectile()
    {
        GameObject instantiatedObj = Object.Instantiate(attackAsset.projectilePrefab, controllerScript.transform.position, Quaternion.identity);
        FireballBehaviour fireball = instantiatedObj.GetComponent<FireballBehaviour>();
        fireball.target = focusedTargetTransform;
    }
}
