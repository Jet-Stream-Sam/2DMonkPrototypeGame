using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveKickAttack : MonoBehaviour, IMoveBehaviour
{
    private PlayerMainController controllerScript;
    private PlayerMoves attackAsset;
    [SerializeField] private float diveHorizontalPower = 3;
    [SerializeField] private float diveVerticalPower = 1;

    private AfterImageEffectPool vfxPool;
    [SerializeField] private GameObject afterImageEffect;
    [SerializeField] private float allowedDistanceBtwImages = 0.8f;
    private float lastImagePos;

    public void Init(PlayerMainController controllerScript, PlayerMoves attackAsset)
    {
        this.controllerScript = controllerScript;
        this.attackAsset = attackAsset;
    }
    public void OnMoveEnter()
    {
        float playerDirection = controllerScript.playerSpriteTransform.localScale.x;
        
        controllerScript.playerRigidBody.velocity =
            new Vector2(playerDirection * diveHorizontalPower + controllerScript.playerRigidBody.velocity.x, 
            -diveVerticalPower + controllerScript.playerRigidBody.velocity.y);


        vfxPool = controllerScript.playerMainVFXManager.afterImageEffectPool;
        vfxPool.UpdatePool(afterImageEffect);
        vfxPool.GetFromPool();
        lastImagePos = controllerScript.playerMainCollider.transform.position.x;
        

    }

    public void OnMoveUpdate()
    {
        
    }

    public void OnMoveFixedUpdate()
    {
        if (Mathf.Abs(controllerScript.playerMainCollider.transform.position.x - lastImagePos) > allowedDistanceBtwImages)
        {
            vfxPool.GetFromPool();
            lastImagePos = controllerScript.playerMainCollider.transform.position.x;
        }
    }

    public void OnMoveExit()
    {

    }
}
