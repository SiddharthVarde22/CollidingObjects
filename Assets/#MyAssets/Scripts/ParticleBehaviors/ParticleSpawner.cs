using System.Threading.Tasks;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour, IUpdatable
{
    [SerializeField]
    ParticleBehavior m_particleToSpawn;
    [SerializeField]
    int m_totalNumberOfParticlesToSpawn;
    [SerializeField]
    Vector3 m_positionToSpawnAt = Vector3.zero;
    [SerializeField]
    bool m_shouldUpdate = true;
    [SerializeField]
    private int m_particlesToSpawnAtOnce = 50;

    private int m_spawnedParticles = 0;

    public bool ShouldUpdate { get { return m_shouldUpdate; } }

    private void Start()
    {
        //for(int i = 0; i < m_numberOfParticlesToSpawn; i++)
        //{
        //    Instantiate(m_particleToSpawn, m_positionToSpawnAt, Quaternion.identity, transform);
        //    //await Task.Delay(1);
        //}

        UpdateManager.SubscribeForUpdateCall(this);
    }

    protected void OnDestroy()
    {
        UpdateManager.UnsubscribeFromUpdateCall(this);
    }

    public void OnUpdateCalled(float a_deltaTime)
    {
        if (m_spawnedParticles < m_totalNumberOfParticlesToSpawn)
        {
            for (int i = 0; m_spawnedParticles < m_totalNumberOfParticlesToSpawn && i < m_particlesToSpawnAtOnce; i++)
            {
                Instantiate(m_particleToSpawn, m_positionToSpawnAt, Quaternion.identity, transform);
                m_spawnedParticles++;
            }
        }
    }
}
