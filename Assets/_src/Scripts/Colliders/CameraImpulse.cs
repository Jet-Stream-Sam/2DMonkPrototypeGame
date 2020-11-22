using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraImpulse : MonoBehaviour
{
    [SerializeField] private HitCheck mainHitBox;

    private void Start()
    {
        mainHitBox.OnSucessfulHit += ApplyImpulse;
    }

    private void ApplyImpulse(Vector3 pos, IDamageable hitBox)
    {
        Cinemachine.CinemachineImpulseSource impulseSource =
                mainHitBox.HitProperties.impulseSource.GetComponent<Cinemachine.CinemachineImpulseSource>();
        impulseSource?.GenerateImpulse(transform.up);
    }

    private void OnDestroy()
    {
        mainHitBox.OnSucessfulHit -= ApplyImpulse;
    }
}
