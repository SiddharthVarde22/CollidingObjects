using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner_Custom : MonoBehaviour, IUpdatable
{
    [SerializeField]
    int m_totalNumberOfParticlesToSpawn;
    [SerializeField]
    bool m_spawnAtRandomPosition = true;
    [SerializeField]
    Vector3 m_positionToSpawnAt = Vector3.zero;
    [SerializeField]
    float m_distanceFromBoundry = 1;
    [SerializeField]
    bool m_shouldUpdate = true;
    [SerializeField]
    private int m_particlesToSpawnAtOnce = 50;


    [SerializeField]
    bool m_particleShouldUpdateFromStart = true;
    [SerializeField]
    float m_particleSpeed = 1, m_particleScale = 0.2f;


    private int m_spawnedParticles = 0;
    Vector2 m_bottomLeftBoundry, m_topRightBoundy;
    List<Matrix4x4> m_particlesSpawnedInFrame;

    public bool ShouldUpdate { get { return m_shouldUpdate; } }

    private void Start()
    {
        (m_bottomLeftBoundry, m_topRightBoundy) = BoundriesCalculator.GetBounries;
        UpdateManager.SubscribeForUpdateCall(this);
        Particle_Drawer.InitializeList(m_totalNumberOfParticlesToSpawn);
        m_particlesSpawnedInFrame = new List<Matrix4x4>(m_particlesToSpawnAtOnce);
    }

    private void OnDestroy()
    {
        UpdateManager.UnsubscribeFromUpdateCall(this);
    }

    public void OnUpdateCalled(float a_deltaTime)
    {
        if (m_spawnedParticles < m_totalNumberOfParticlesToSpawn)
        {
            for (int i = 0; m_spawnedParticles < m_totalNumberOfParticlesToSpawn && i < m_particlesToSpawnAtOnce; i++)
            {
                if (m_spawnAtRandomPosition)
                {
                    m_positionToSpawnAt.x = Random.Range(m_bottomLeftBoundry.x + m_distanceFromBoundry, m_topRightBoundy.x - m_distanceFromBoundry);
                    m_positionToSpawnAt.y = Random.Range(m_bottomLeftBoundry.y + m_distanceFromBoundry, m_topRightBoundy.y - m_distanceFromBoundry);
                }

                Particle_CustomDraw l_spawnedParticle = new Particle_CustomDraw
                    (m_particleShouldUpdateFromStart, m_particleSpeed, m_particleScale, m_positionToSpawnAt, m_spawnedParticles);
                m_particlesSpawnedInFrame.Add(l_spawnedParticle.GetWorldMatrix());

                m_spawnedParticles++;
            }
            Particle_Drawer.AddRangeOfParticles(m_particlesSpawnedInFrame);
            m_particlesSpawnedInFrame.Clear();
        }
    }
}
