using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveKickAttack : MonoBehaviour, IAttackBehaviour
{
    private PlayerMainController controllerScript;
    [SerializeField] private float diveHorizontalPower = 3;
    [SerializeField] private float diveVerticalPower = 1;

    [SerializeField] private float allowedDistanceBtwImages = 0.8f;
    private float lastImagePos;

    public void Init(PlayerMainController controllerScript)
    {
        this.controllerScript = controllerScript;
    }
    public void OnAttackEnter()
    {
        float playerDirection = controllerScript.playerSpriteTransform.localScale.x;
        
        controllerScript.playerRigidBody.velocity =
            new Vector2(playerDirection * diveHorizontalPower + controllerScript.playerRigidBody.velocity.x, 
            -diveVerticalPower + controllerScript.playerRigidBody.velocity.y);

        controllerScript.afterImageEffectPool.GetFromPool();
        lastImagePos = controllerScript.playerMainCollider.transform.position.x;
        

    }

    public void OnAttackUpdate()
    {
        
    }

    public void OnAttackFixedUpdate()
    {
        if (Mathf.Abs(controllerScript.playerMainCollider.transform.position.x - lastImagePos) > allowedDistanceBtwImages)
        {
            controllerScript.afterImageEffectPool.GetFromPool();
            lastImagePos = controllerScript.playerMainCollider.transform.position.x;
        }
    }

    public void OnAttackExit()
    {

    }
}
