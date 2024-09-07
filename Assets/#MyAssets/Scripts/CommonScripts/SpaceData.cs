using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

[System.Serializable]
public class SpaceData : IJobUpdatable
{
    Vector2 m_topLeftCorner, m_bottomRightCorner;
    List<ISpaceable> m_particles = new List<ISpaceable>();
    bool m_isAtEdge;
    bool m_shouldUpdate = true;

    public bool ShouldUpdate()
    {
        return m_shouldUpdate;
    }
    public IReadOnlyList<ISpaceable> ParticlesInSpace { get { return m_particles; } }
    public bool IsAtEdge { get { return m_isAtEdge; } }
    public int CurrentNumberOfParticles { get { return m_particles.Count; } }

    CollisionCheckJob m_collisionCheckJob;
    JobHandle m_collisionCheckJobHandle;
    NativeArray<Vector3> m_particlePositions;
    NativeArray<Vector3> m_particleDirections;
    NativeArray<float> m_particleScale;
    

    public SpaceData()
    {
        UpdateManager.SubscribeForJobUpdateCall(this);
    }
    ~SpaceData()
    {
        UpdateManager.UnsubscribeFromJobUpdateCall(this);
    }

    public void OnUpdateCalled(float a_deltaTime)
    {
        int l_count = m_particles.Count;
        m_particlePositions = new NativeArray<Vector3>(l_count, Allocator.TempJob);
        m_particleDirections = new NativeArray<Vector3>(l_count, Allocator.TempJob);
        m_particleScale = new NativeArray<float>(l_count, Allocator.TempJob);
        ISpaceable l_particle;
        for(int i = 0; i < l_count; i++)
        {
            l_particle = m_particles[i];
            m_particlePositions[i] = l_particle.GetPosition();
            m_particleDirections[i] = l_particle.GetSpeedDirection();
            m_particleScale[i] = l_particle.GetScale();
        }

        m_collisionCheckJob = new CollisionCheckJob
        {
            m_particlesPositionsInSpace = m_particlePositions,
            m_particleSpeedDirection = m_particleDirections,
            m_scale = m_particleScale
        };
        m_collisionCheckJobHandle = m_collisionCheckJob.Schedule();

    }

    public void OnJobCompleteCalled()
    {
        int l_count = m_particles.Count;
        ISpaceable l_particle;

        m_collisionCheckJobHandle.Complete();

        for (int i = 0; i < l_count; i++)
        {
            l_particle = m_particles[i];
            l_particle.SetSpeedDirection(m_collisionCheckJob.m_particleSpeedDirection[i]);
        }

        m_particlePositions.Dispose();
        m_particleDirections.Dispose();
        m_particleScale.Dispose();
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

    public void AddParticle(ISpaceable a_particleToAdd)
    {
        //if (m_particles.Contains(a_particleToAdd))
        //{
        //    Debug.LogError(" Already exists in list");
        //    return;
        //}

        m_particles.Add(a_particleToAdd);
    }

    public void RemoveParticle(ISpaceable a_particleToRemove)
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

    public ISpaceable GetParticle(int index)
    {
        return m_particles[index];
    }
}
