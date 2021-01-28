using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem.LowLevel;

public class ParticleLimiter : MonoBehaviour
{
    [Required][SerializeField] private ParticleSystem mainParticleSystem;
    private ParticleSystem.EmissionModule emissionModule;
    [Range(0, 1)][SerializeField] private float limiterRate = 0.5f;
    [SerializeField] private bool limitBursts = false;
    [ShowIf("limitBursts")]
    [Range(0, 1)] [SerializeField] private float burstLimiterRate = 0.5f;
    private ParticleSystem.MinMaxCurve originalRateOverTime;
    private ParticleSystem.MinMaxCurve originalRateOverDistance;
    private int limitValue = 0;
    

    private void Start()
    {
        originalRateOverTime = mainParticleSystem.emission.rateOverTime;
        originalRateOverDistance = mainParticleSystem.emission.rateOverDistance;
        emissionModule = mainParticleSystem.emission;

        limitValue = ParticleLevelSetting.ParticleLevel;
        LimitRates();
        LimitBursts();
        ParticleLevelSetting.OnSettingsChanged += ChangeSetting;
   
    }

    private void ChangeSetting(int setting)
    {
        limitValue = setting;

        LimitRates();
    }

    private void LimitRates()
    {
        switch (limitValue)
        {
            case 0:
                emissionModule.rateOverTime = originalRateOverTime;
                emissionModule.rateOverDistance = originalRateOverDistance;
                break;
            case 1:
                emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(
                    originalRateOverTime.constantMax * limiterRate);
                emissionModule.rateOverDistance = new ParticleSystem.MinMaxCurve(
                    originalRateOverDistance.constantMax * limiterRate);
                break;

        }
    }

    private void LimitBursts()
    {
        if (limitValue == 1 && limitBursts)
        {
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emissionModule.burstCount];
            emissionModule.GetBursts(bursts);

            for (int i = 0; i < bursts.Length; i++)
            {
                bursts[i].count = new ParticleSystem.MinMaxCurve(
            bursts[i].count.constantMax * burstLimiterRate);
            }
            emissionModule.SetBursts(bursts);
        }
    }
    private void OnDestroy()
    {
        ParticleLevelSetting.OnSettingsChanged -= ChangeSetting;
    }
}
