using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveKickAttack : MonoBehaviour, IAttackBehaviour
{
    private PlayerMainController controllerScript;
    [SerializeField] private float diveHorizontalPower = 3;
    [SerializeField] private float diveVerticalPower = 1;
    public void Init(PlayerMainController controllerScript)
    {
        this.controllerScript = controllerScript;
    }
    public void OnAttackEnter()
    {
        float playerDirection = controllerScript.playerSpriteTransform.localScale.x;
        
        //controllerScript.playerRigidBody.velocity = Vector2.right * playerDirection * divePower;
        /*controllerScript.playerRigidBody.AddForce
            (new Vector2(playerDirection * diveHorizontalPower, controllerScript.playerRigidBody.velocity.y * ), ForceMode2D.Impulse);*/
        controllerScript.playerRigidBody.velocity =
            new Vector2(playerDirection * diveHorizontalPower + controllerScript.playerRigidBody.velocity.x, 
            -diveVerticalPower + controllerScript.playerRigidBody.velocity.y);
        Debug.Log(controllerScript.playerRigidBody.velocity);

    }

    public void OnAttackUpdate()
    {

    }

    public void OnAttackFixedUpdate()
    {
        
    }

    public void OnAttackExit()
    {

    }
}
