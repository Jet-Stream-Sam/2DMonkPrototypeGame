using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Collider2D deathZoneTrigger;
    [SerializeField] private SpawnPoint spawnPointReference;
    [SerializeField] private int zoneDamage;
    private PlayerMainTrigger player;
    private RigidbodyConstraints2D previousRigidbodyConstraints;
    private bool isRespawing;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isRespawing)
            return;
        if(col.TryGetComponent(out player))
        {
            player.TakeDamage(zoneDamage, Vector2.up, 0);
            var playerRigidBody = player.playerController.playerRigidBody;
            previousRigidbodyConstraints = playerRigidBody.constraints;
            playerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(WaitUntilRespawn(0.5f));
            
        }
        

    }

    IEnumerator WaitUntilRespawn(float seconds)
    {
        isRespawing = true;
        var playerRigidBody = player.playerController.playerRigidBody;
        yield return new WaitForSeconds(seconds);
        playerRigidBody.constraints = previousRigidbodyConstraints;
        spawnPointReference.MoveToSpawn(player.transform);
        isRespawing = false;
    }
}
