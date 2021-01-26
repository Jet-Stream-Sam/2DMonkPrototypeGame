using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendImpulse : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;
    public void TriggerImpulse()
    {
        impulseSource.GenerateImpulse(transform.up);
    }
}
