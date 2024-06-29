using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_CustomDraw : IUpdatable, ISpaceble
{
    bool m_shouldUpdate;
    float m_movementSpeed;

    Matrix4x4 m_worldMatix;

    Vector3 m_speedDirection;
    Vector2 m_bottomLeftCorner, m_topRightCorner;
    Vector3 m_currentPosition;
    //Vector3 m_scale;
    float m_scale;
    //Quaternion m_rotation;
    SpaceData m_currentSpace;
    int m_indexToGetDrawn;
    //float m_bottomLeftLimit, m_bottomRightLimit, m_topLeftLimit, m_topRightLimit; 
    //bool m_shouldCheckForBoundries = false;

    public bool ShouldUpdate { get { return m_shouldUpdate; } }

    public Particle_CustomDraw(bool a_shouldUpdate, float a_speed, float a_scale, Vector3 a_position, int a_indexTogetDrawn)
    {
        m_shouldUpdate = a_shouldUpdate;
        m_movementSpeed = a_speed;
        
        //m_worldMatix.position = a_position;
        m_currentPosition = a_position;
        m_scale = a_scale;
        //m_rotation = Quaternion.identity;
        m_worldMatix.SetTRS(a_position, Quaternion.identity, Vector3.one * a_scale);

        m_speedDirection = (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0)).normalized;
        (m_bottomLeftCorner, m_topRightCorner) = BoundriesCalculator.GetBounries;
        m_bottomLeftCorner.x += m_scale;
        m_bottomLeftCorner.y += m_scale;
        m_topRightCorner.x -= m_scale;
        m_topRightCorner.y -= m_scale;
        m_indexToGetDrawn = a_indexTogetDrawn;

        UpdateManager.SubscribeForUpdateCall(this);
        GetAddedToSpace();
    }

    ~Particle_CustomDraw()
    {
        UpdateManager.UnsubscribeFromUpdateCall(this);
    }

    public void OnUpdateCalled(float a_deltaTime)
    {
        //m_currentPosition += (m_speedDirection * (m_movementSpeed * a_deltaTime));
        m_currentPosition.x += (m_speedDirection.x * m_movementSpeed * a_deltaTime);
        m_currentPosition.y += (m_speedDirection.y * m_movementSpeed * a_deltaTime);
        //m_worldMatix.SetTRS(m_currentPosition, m_rotation, m_scale);

        if (!m_currentSpace.CheckIfPositionExistsInSpace(m_currentPosition))
        {
            GetRemovedFromSpace();
            GetAddedToSpace();
        }

        if (m_currentSpace.IsAtEdge)
        {
            CheckForBoundries();
        }
        CheckForCollisionInSpace();

        Particle_Drawer.UpdateParticleMatrixInList(m_indexToGetDrawn, m_currentPosition);
    }

    private void CheckForBoundries()
    {
        if (m_currentPosition.x <= (m_bottomLeftCorner.x))
        {
            m_speedDirection.x *= -1;
            m_currentPosition.x = m_bottomLeftCorner.x;
        }
        else if (m_currentPosition.x >= (m_topRightCorner.x))
        {
            m_speedDirection.x *= -1;
            m_currentPosition.x = m_topRightCorner.x;
        }

        if (m_currentPosition.y <= (m_bottomLeftCorner.y))
        {
            m_speedDirection.y *= -1;
            m_currentPosition.y = m_bottomLeftCorner.y;
        }
        else if (m_currentPosition.y >= (m_topRightCorner.y))
        {
            m_speedDirection.y *= -1;
            m_currentPosition.y = m_topRightCorner.y;
        }

        //m_worldMatix.SetColumn(3, m_currentPosition);
    }

    Vector3 m_directionFromParticle;
    float m_distanceFromParticle;
    private void CheckForCollisionInSpace()
    {
        ISpaceble l_otherParticle;
        Vector3 l_otherParticlePos;
        float l_currentNumberOfParticles = m_currentSpace.CurrentNumberOfParticles;
        for (int i = 0; i < l_currentNumberOfParticles; i++)
        {
            l_otherParticle = m_currentSpace.ParticlesInSpace[i];
            if (l_otherParticle == this)
            {
                continue;
            }
            l_otherParticlePos = l_otherParticle.GetPosition();
            //m_directionFromParticle = l_otherParticle.GetPosition() - m_currentPosition;
            m_directionFromParticle.x = l_otherParticlePos.x - m_currentPosition.x;
            m_directionFromParticle.y = l_otherParticlePos.y - m_currentPosition.y;
            //m_directionFromParticle.z = l_otherParticlePos.z - m_currentPosition.z;

            m_distanceFromParticle = Mathf.Sqrt(m_directionFromParticle.x * m_directionFromParticle.x 
                + m_directionFromParticle.y * m_directionFromParticle.y);

            if (m_distanceFromParticle <= m_scale)
            {
                //m_speedDirection = (m_directionFromParticle / -m_distanceFromParticle);
                m_speedDirection.x = (m_directionFromParticle.x / -m_distanceFromParticle);
                m_speedDirection.y = (m_directionFromParticle.y / -m_distanceFromParticle);
                return;
            }
        }
    }

    private void GetAddedToSpace()
    {
        m_currentSpace = SpacePartitions.GetSpace(m_currentPosition);
        //if (m_currentSpace == null)
        //{
        //    Debug.LogError("Current space is null");
        //}
        m_currentSpace.AddParticle(this);
        //m_shouldCheckForBoundries = m_currentSpace
    }

    private void GetRemovedFromSpace()
    {
        //if (m_currentSpace != null)
        //{
        //}
        m_currentSpace.RemoveParticle(this);

    }

    public Vector3 GetPosition()
    {
        return m_currentPosition;
    }

    public Matrix4x4 GetWorldMatrix()
    {
        return m_worldMatix;
    }
}
