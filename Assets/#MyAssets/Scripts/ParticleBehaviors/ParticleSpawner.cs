using System.Threading.Tasks;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour, IUpdatable
{
    [SerializeField]
    ParticleBehavior m_particleToSpawn;
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

    private int m_spawnedParticles = 0;
    Vector2 m_bottomLeftBoundry, m_topRightBoundy;
    public bool ShouldUpdate { get { return m_shouldUpdate; } }

    private void Start()
    {
        //for(int i = 0; i < m_numberOfParticlesToSpawn; i++)
        //{
        //    Instantiate(m_particleToSpawn, m_positionToSpawnAt, Quaternion.identity, transform);
        //    //await Task.Delay(1);
        //}
        (m_bottomLeftBoundry, m_topRightBoundy) = BoundriesCalculator.GetBounries;
        //Debug.LogError(m_bottomLeftBoundry + " , " + m_topRightBoundy);
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
                if (m_spawnAtRandomPosition)
                {
                    m_positionToSpawnAt.x = Random.Range(m_bottomLeftBoundry.x + m_distanceFromBoundry, m_topRightBoundy.x - m_distanceFromBoundry);
                    m_positionToSpawnAt.y = Random.Range(m_bottomLeftBoundry.y + m_distanceFromBoundry, m_topRightBoundy.y - m_distanceFromBoundry);
                }
                Instantiate(m_particleToSpawn, m_positionToSpawnAt, Quaternion.identity, transform);
                m_spawnedParticles++;
            }
        }
    }
}
