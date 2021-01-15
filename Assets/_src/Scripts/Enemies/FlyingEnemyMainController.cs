using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FlyingEnemyMainController : EnemyMainController, IDamageable, IEntityController
{
    private void Start()
    {
        SoundManager = SoundManager.Instance;
        currentHealth = maxHealth;

        StateMachine = new MainStateMachine();

        StateMachine.onStateChanged += state => currentStateOutput = state;
        StateMachine.Init(new FlyingEnemyIdleState(this, StateMachine));

    }

    public override void TakeDamage(int damage, Vector2 forceDirection, float knockbackForce)
    {
        base.TakeDamage(damage, forceDirection, knockbackForce);

        StateMachine.ChangeState(new FlyingEnemyHitStunnedState(this, StateMachine));
    }


}
