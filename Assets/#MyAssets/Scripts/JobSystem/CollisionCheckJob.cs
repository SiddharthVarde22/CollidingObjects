using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

public struct CollisionCheckJob : IJob
{
    public NativeArray<Vector3> m_particlesPositionsInSpace;
    public NativeArray<Vector3> m_particleSpeedDirection;
    public NativeArray<float> m_scale;


    public void Execute()
    {
        int l_length = m_particlesPositionsInSpace.Length;
        Vector3 l_currentParticlePos, l_currentDirection, l_otherParticlePos;
        float l_rightCollisionPos, l_leftCollisionPos, l_upCollisionPos, l_downCollisionPos;

        for(int i = 0; i < l_length; i++)
        {
            l_currentParticlePos = m_particlesPositionsInSpace[i];
            l_currentDirection = m_particleSpeedDirection[i];
            l_rightCollisionPos = l_currentParticlePos.x + m_scale[i];
            l_leftCollisionPos = l_currentParticlePos.x - m_scale[i];
            l_upCollisionPos = l_currentParticlePos.y + m_scale[i];
            l_downCollisionPos = l_currentParticlePos.y - m_scale[i];

            for (int j = 0; j < l_length; j++)
            {
                if(i == j)
                {
                    continue;
                }

                l_otherParticlePos = m_particlesPositionsInSpace[j];

                if ((l_otherParticlePos.x > l_rightCollisionPos || l_otherParticlePos.x < l_leftCollisionPos) ||
                    (l_otherParticlePos.y > l_upCollisionPos || l_otherParticlePos.y < l_downCollisionPos))
                {
                    continue;
                }

                Vector3 l_directionFromPraticle;
                l_directionFromPraticle.x = l_currentParticlePos.x - l_otherParticlePos.x;
                l_directionFromPraticle.y = l_currentParticlePos.y - l_otherParticlePos.y;
                l_directionFromPraticle.z = l_currentDirection.z;

                float distance = math.sqrt(l_directionFromPraticle.x * l_directionFromPraticle.x
                    + l_directionFromPraticle.y * l_directionFromPraticle.y);
                l_directionFromPraticle.x /= distance;
                l_directionFromPraticle.y /= distance;

                m_particleSpeedDirection[i] = l_directionFromPraticle;
                break;
            }
        }
    }
}
