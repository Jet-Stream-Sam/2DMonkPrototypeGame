using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DestroyParticleObjectIfHasEnded : MonoBehaviour
{
    public enum DestructionType
    {
        ParticlesDeath,
        Duration
    }

    [SerializeField] private DestructionType destructionType;
    [SerializeField] private ParticleSystem objParticleSystem;
    private float durationTimer;

    private void Start()
    {
        if (destructionType == DestructionType.Duration)
            StartCoroutine(RunThroughDuration(objParticleSystem.main.duration));
    }
    private void Update()
    {
        switch (destructionType)
        {
            case DestructionType.ParticlesDeath:
                if (!objParticleSystem.IsAlive())
                    Destroy(objParticleSystem.gameObject);
                break;
            
        }
        
    }

    private IEnumerator RunThroughDuration(float duration)
    {
        durationTimer = duration;

        while (true)
        {
            durationTimer -= Time.deltaTime;
            if (durationTimer < 0) break;
            yield return null;
        }
        Destroy(objParticleSystem.gameObject);
    }
}
