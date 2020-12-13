using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCameraImpulse : MonoBehaviour
{
    [SerializeField] private HitCheck mainHitBox;

    private void Start()
    {
        if (mainHitBox == null)
            return;
        mainHitBox.OnSucessfulHit += ApplyImpulse;
    }

    private void ApplyImpulse(Vector3 pos, IDamageable hitBox)
    {
        if (hitBox == null)
            return;

        Cinemachine.CinemachineImpulseSource impulseSource =
                mainHitBox.HitProperties.impulseSource.GetComponent<Cinemachine.CinemachineImpulseSource>();
        impulseSource?.GenerateImpulse(transform.up);
    }

    private void OnDestroy()
    {
        if (mainHitBox == null)
            return;
        mainHitBox.OnSucessfulHit -= ApplyImpulse;
    }
}
