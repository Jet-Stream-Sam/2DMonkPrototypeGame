using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject m_Prefab;

    private ParticleSystem m_ParticleSystem;
    private List<GameObject> m_Instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_Particles;

    [SerializeField] private bool sizeAffectsRange = true;
    [SerializeField] private bool alphaAffectsIntensity = true;
    [SerializeField] private bool endIfNotAlive = true;
    [ShowIf("sizeAffectsRange")]
    [SerializeField] private float sizeMultiplier = 1;
    [ShowIf("alphaAffectsIntensity")]
    [SerializeField] private float alphaMultiplier = 1;

    private void Awake()
    {
        if (ParticleLightingSetting.ParticleLighting == 1)
        {
            Destroy(this);
        }
    }
    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
    }

    void LateUpdate()
    {
        if (m_Prefab == null) return;

        if (endIfNotAlive && !m_ParticleSystem.IsAlive()) Destroy(gameObject);

        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count)
            m_Instances.Add(Instantiate(m_Prefab, m_ParticleSystem.transform));

        bool worldSpace = (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_Instances.Count; i++)
        {
            if (i < count)
            {
                Light2D light = m_Instances[i].GetComponent<Light2D>();
                if (sizeAffectsRange)
                {
                    light.pointLightOuterRadius = m_Particles[i].GetCurrentSize(m_ParticleSystem) * sizeMultiplier;
                }
                if (alphaAffectsIntensity)
                {
                    float particleAlpha = m_Particles[i].GetCurrentColor(m_ParticleSystem).a;
                    float normalizedLightValue = Mathf.InverseLerp(0, m_Particles[i].startColor.a, particleAlpha);
                    float resultLightValue = Mathf.Lerp(0, 1, normalizedLightValue);
                    light.intensity = resultLightValue * alphaMultiplier;
                }
                
                
                if (worldSpace)
                    m_Instances[i].transform.position = m_Particles[i].position;
                else
                    m_Instances[i].transform.localPosition = m_Particles[i].position;
                m_Instances[i].SetActive(true);
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
        }
    }
}
