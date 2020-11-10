using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningPunchAttack : MonoBehaviour, IAttackBehaviour
{
    private PlayerMainController controllerScript;
    [SerializeField] private float dashPower = 7;
    [SerializeField] private float easingRate = 0.8f;
    private float easingMovement;

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
            new Vector2(playerDirection * dashPower + controllerScript.playerRigidBody.velocity.x,
            controllerScript.playerRigidBody.velocity.y);

        easingMovement = controllerScript.playerRigidBody.velocity.x;

        controllerScript.afterImageEffectPool.GetFromPool();
        lastImagePos = controllerScript.playerMainCollider.transform.position.x;
    }

    public void OnAttackUpdate()
    {
        

        
    }

    public void OnAttackFixedUpdate()
    {
        easingMovement =
            Mathf.Lerp(easingMovement,
            0,
            easingRate);

        float tempSpeed = easingMovement;

        controllerScript.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * controllerScript.fallMultiplier * Time.deltaTime;

        controllerScript.playerRigidBody.velocity =
            new Vector2(tempSpeed, controllerScript.playerRigidBody.velocity.y);

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
