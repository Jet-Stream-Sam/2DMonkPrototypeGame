using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingKickAttack : MonoBehaviour, IAttackBehaviour
{
    private PlayerMainController controllerScript;
    private PlayerAttack attackAsset;

    [SerializeField] private float dashPower = 1;
    [SerializeField] private float easingRate = 0.8f;

    [SerializeField] private GameObject afterImageEffect;
    [SerializeField] private float allowedDistanceBtwImages = 0.8f;

    
    private float easingMovement;
    private float lastImagePos;

    private AfterImageEffectPool vfxPool;

    public void Init(PlayerMainController controllerScript, PlayerAttack attackAsset)
    {
        this.controllerScript = controllerScript;
        this.attackAsset = attackAsset;
    }

    public void OnAttackEnter()
    {
        float playerDirection = controllerScript.playerSpriteTransform.localScale.x;

        controllerScript.playerRigidBody.velocity =
            new Vector2(playerDirection * dashPower + controllerScript.playerRigidBody.velocity.x,
            controllerScript.playerRigidBody.velocity.y);

        easingMovement = controllerScript.playerRigidBody.velocity.x;

        vfxPool = controllerScript.VFXTransform.GetComponentInChildren<AfterImageEffectPool>();
        vfxPool.UpdatePool(afterImageEffect);
        vfxPool.GetFromPool();
    }

    public void OnAttackExit()
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
            vfxPool.GetFromPool();
            lastImagePos = controllerScript.playerMainCollider.transform.position.x;
        }
    }

    public void OnAttackUpdate()
    {

    }
}
