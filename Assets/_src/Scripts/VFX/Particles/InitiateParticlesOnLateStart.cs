using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateParticlesOnLateStart : MonoBehaviour
{
    [SerializeField] private ParticleSystem mainParticleSystem;

    private void Start()
    {
        StartCoroutine(SkipFrame(() => { mainParticleSystem.Play(); }));
    }

    private IEnumerator SkipFrame(Action func)
    {
        yield return null;

        func?.Invoke();
    }
}
