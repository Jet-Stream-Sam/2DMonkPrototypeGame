using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRef : MonoBehaviour, IDamageable
{
    public PlayerMainController playerController;

    public void TakeDamage(int damage)
    {
        playerController.TakeDamage(damage);
    }
    public void TakeDamage(int damage, Vector2 forceDirection, float knockbackForce)
    {
        playerController.TakeDamage(damage, forceDirection, knockbackForce);
    }

    public void AnimationSendObject(ScriptableObject obj)
    {
        playerController.AnimationSendObject(obj);
    }
}
