using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalBehavior_Mannual : ParticleBehavior, IUpdatable, ISpaceble
{
    [SerializeField]
    bool m_shouldUpdate = true;
    [SerializeField]
    float m_movementSpeed = 1;
    //[SerializeField]
    //float m_extraDistanceToMainTain = 0.05f;
    //[SerializeField]
    //float m_timeToDectivateCollisionWithParticles = 0.02f;

    float m_radius;
    Vector3 m_speedDirection;
    Vector2 m_bottomLeftCorner, m_topRightCorner;
    Vector3 m_currentPosition;

    SpaceData m_currentSpace;

    public bool ShouldUpdate { get { return m_shouldUpdate; } }

    public override void AddForce()
    {
        m_speedDirection = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0)).normalized;
    }

    protected override void Start()
    {
        base.Start();
        UpdateManager.SubscribeForUpdateCall(this);
        (m_bottomLeftCorner, m_topRightCorner) = BoundriesCalculator.GetBounries;
        m_radius = transform.localScale.x;
        GetAddedToSpace();
    }

    private void OnDestroy()
    {
        UpdateManager.UnsubscribeFromUpdateCall(this);
        GetRemovedFromSpace();
    }

    public void OnUpdateCalled(float a_deltaTime)
    {
        transform.position += (m_speedDirection * (m_movementSpeed * a_deltaTime));
        m_currentPosition = transform.position;
        CheckForBoundries();

        if (!m_currentSpace.CheckIfPositionExistsInSpace(m_currentPosition))
        {
            GetRemovedFromSpace();
            GetAddedToSpace();
        }

        CheckForCollisionInSpace();
    }

    private void CheckForBoundries()
    {
        if(m_currentPosition.x <= (m_bottomLeftCorner.x + m_radius))
        {
            m_speedDirection.x *= -1;
            m_currentPosition.x = m_bottomLeftCorner.x + m_radius;
        }
        else if(m_currentPosition.x >= (m_topRightCorner.x - m_radius))
        {
            m_speedDirection.x *= -1;
            m_currentPosition.x = m_topRightCorner.x - m_radius;
        }

        if (m_currentPosition.y <= (m_bottomLeftCorner.y + m_radius))
        {
            m_speedDirection.y *= -1;
            m_currentPosition.y = m_bottomLeftCorner.y + m_radius;
        }
        else if (m_currentPosition.y >= (m_topRightCorner.y - m_radius))
        {
            m_speedDirection.y *= -1;
            m_currentPosition.y = m_topRightCorner.y - m_radius;
        }

        transform.position = m_currentPosition;
    }

    Vector3 l_direction;
    //Vector3 l_particlePosition;
    float l_distance;
    private void CheckForCollisionInSpace()
    {
        //IReadOnlyList<Transform> l_particles = m_currentSpace.ParticlesInSpace;
        

        for (int i = 0; i < m_currentSpace.ParticlesInSpace.Count; i++)
        {
            ISpaceble otherParticle = m_currentSpace.ParticlesInSpace[i];
            if(otherParticle as ParticalBehavior_Mannual == this)
            {
                continue;
            }

            //l_particlePosition = m_currentSpace.ParticlesInSpace[i].position;
            l_direction = otherParticle.GetPosition() - m_currentPosition;
            l_distance = Mathf.Sqrt(l_direction.x * l_direction.x + l_direction.y * l_direction.y);

            if(l_distance <= m_radius)
            //if(Vector3.Distance(l_particlePosition, m_currentPosition) <= (m_radius * 2))
            {
                //if (m_currentPosition.x <= l_particlePosition.x || m_currentPosition.x >= l_particlePosition.x)
                //{
                //    m_speedDirection.x *= -1;
                //    return;
                //}

                //if (m_currentPosition.y <= l_particlePosition.y || m_currentPosition.y >= l_particlePosition.y)
                //{
                //    m_speedDirection.y *= -1;
                //    return;
                //}

                m_speedDirection = (l_direction / -l_distance);
                return;
            }
        }
    }

    private void GetAddedToSpace()
    {
        m_currentSpace = SpacePartitions.GetSpace(m_currentPosition);
        if(m_currentSpace == null)
        {
            Debug.LogError("Current space is null", gameObject);
        }
        m_currentSpace.AddParticle(this);
    }
    
    private void GetRemovedFromSpace()
    {
        if(m_currentSpace != null)
        {
            m_currentSpace.RemoveParticle(this);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public Vector3 GetDirection()
    {
        return m_speedDirection;
    }
    public float GetSize()
    {
        return m_radius;
    }
    public void SetDirection(Vector3 a_direction)
    {
        m_speedDirection = a_direction;
    }
}
