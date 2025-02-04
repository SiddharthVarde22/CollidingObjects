using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceData : ILateUpdatable
{
    Vector2 m_topLeftCorner, m_bottomRightCorner;
    List<ISpaceble> m_particles = new List<ISpaceble>();
    bool m_isAtEdge;
    bool m_shouldUpdate = true;

    public IReadOnlyList<ISpaceble> ParticlesInSpace { get { return m_particles; } }
    public bool IsAtEdge { get { return m_isAtEdge; } }
    public int CurrentNumberOfParticles { get { return m_particles.Count; } }

    public SpaceData()
    {
        m_shouldUpdate = true;
        UpdateManager.SubscribeToLateUpdateCallback(this);
    }
    ~SpaceData()
    {
        UpdateManager.UnSubscribeFromLateUpdate(this);
    }

    public void Init(Vector2 a_topLeftCorner, float a_lengthOfSpace, bool a_isAtEdge)
    {
        m_topLeftCorner = a_topLeftCorner;

        Vector2 l_bottomRightCorner = a_topLeftCorner;
        l_bottomRightCorner.x += a_lengthOfSpace;
        l_bottomRightCorner.y -= a_lengthOfSpace;

        m_bottomRightCorner = l_bottomRightCorner;
        m_isAtEdge = a_isAtEdge;

        //Debug.LogError(m_topLeftCorner + " , " + m_bottomRightCorner);
    }

    public bool ShouldUpdate()
    {
        return m_shouldUpdate;
    }

    public void OnLateUpdateCalled(float a_deltaTime)
    {
        ISpaceble l_particle;
        int l_count = m_particles.Count;
        Vector3 l_currentParticlePos, l_otherParticlePos;
        float l_maxRight, l_maxLeft, l_maxUp, l_maxDown, l_size;

        for(int i = 0; i < l_count; i++)
        {
            l_particle = m_particles[i];
            l_currentParticlePos = l_particle.GetPosition();
            l_size = l_particle.GetSize();
            l_maxLeft = l_currentParticlePos.x - l_size;
            l_maxRight= l_currentParticlePos.x + l_size;
            l_maxUp = l_currentParticlePos.y + l_size;
            l_maxDown = l_currentParticlePos.y - l_size;


            for (int j = 0; j < l_count; j++)
            {
                if (i == j) continue;

                l_otherParticlePos = m_particles[j].GetPosition();

                if (l_otherParticlePos.x > l_maxRight || l_otherParticlePos.x < l_maxLeft ||
                    l_otherParticlePos.y > l_maxUp || l_otherParticlePos.y < l_maxDown)
                {
                    continue;
                }

                Vector3 l_direction;
                l_direction.x = l_currentParticlePos.x - l_otherParticlePos.x;
                l_direction.y = l_currentParticlePos.y - l_otherParticlePos.y;
                l_direction.z = 0;
                float l_distance = Mathf.Sqrt(l_direction.x * l_direction.x + l_direction.y * l_direction.y);

                l_direction.x /= l_distance;
                l_direction.y /= l_distance;
                l_particle.SetDirection(l_direction);
                break;
            }
        }
    }

    public void AddParticle(ISpaceble a_particleToAdd)
    {
        //if (m_particles.Contains(a_particleToAdd))
        //{
        //    Debug.LogError(" Already exists in list");
        //    return;
        //}

        m_particles.Add(a_particleToAdd);
    }

    public void RemoveParticle(ISpaceble a_particleToRemove)
    {
        m_particles.Remove(a_particleToRemove);
        return;

        //if (m_particles.Contains(a_particleToRemove))
        //{
        //}
        //Debug.LogError(" Does not Exists in list");
    }

    public bool CheckIfPositionExistsInSpace(Vector3 a_position)
    {
        return (CheckIf_X_IsInSpace(a_position.x) && CheckIf_Y_InSpace(a_position.y));
    }

    public bool CheckIf_X_IsInSpace(float a_x)
    {
        return (a_x >= m_topLeftCorner.x && a_x <= m_bottomRightCorner.x);
    }

    public bool CheckIf_Y_InSpace(float a_y)
    {
        return (a_y <= m_topLeftCorner.y && a_y >= m_bottomRightCorner.y);
    }

    public ISpaceble GetParticle(int index)
    {
        return m_particles[index];
    }
}
