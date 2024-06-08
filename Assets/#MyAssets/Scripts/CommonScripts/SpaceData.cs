using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceData
{
    Vector2 m_topLeftCorner, m_bottomRightCorner;
    List<Transform> m_particles = new List<Transform>();

    public IReadOnlyList<Transform> ParticlesInSpace { get { return m_particles; } }

    public void Init(Vector2 a_topLeftCorner, float a_lengthOfSpace)
    {
        m_topLeftCorner = a_topLeftCorner;

        Vector2 l_bottomRightCorner = a_topLeftCorner;
        l_bottomRightCorner.x += a_lengthOfSpace;
        l_bottomRightCorner.y -= a_lengthOfSpace;

        m_bottomRightCorner = l_bottomRightCorner;

        //Debug.LogError(m_topLeftCorner + " , " + m_bottomRightCorner);
    }

    public void AddParticle(Transform a_particleToAdd)
    {
        if (m_particles.Contains(a_particleToAdd))
        {
            Debug.LogError(a_particleToAdd.name + " Already exists in list");
            return;
        }

        m_particles.Add(a_particleToAdd);
    }

    public void RemoveParticle(Transform a_particleToRemove)
    {
        if (m_particles.Contains(a_particleToRemove))
        {
            m_particles.Remove(a_particleToRemove);
            return;
        }
        Debug.LogError(a_particleToRemove.name + " Does not Exists in list", a_particleToRemove.gameObject);
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
}
