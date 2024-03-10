using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalBehavior_Mannual : ParticleBehavior, IUpdatable
{
    [SerializeField]
    bool m_shouldUpdate = true;
    [SerializeField]
    float m_movementSpeed = 1;

    float m_radius;
    Vector3 m_speedDirection;
    Vector2 m_bottomLeftCorner, m_topRightCorner;

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
    }

    public void OnUpdateCalled(float a_deltaTime)
    {
        transform.position += (m_speedDirection * (m_movementSpeed * a_deltaTime));
        CheckForBoundries();
    }

    private void CheckForBoundries()
    {
        if(transform.position.x <= (m_bottomLeftCorner.x + m_radius))
        {
            m_speedDirection.x *= -1;
        }
        else if(transform.position.x >= (m_topRightCorner.x - m_radius))
        {
            m_speedDirection.x *= -1;
        }

        if (transform.position.y <= (m_bottomLeftCorner.y + m_radius))
        {
            m_speedDirection.y *= -1;
        }
        else if (transform.position.y >= (m_topRightCorner.y - m_radius))
        {
            m_speedDirection.y *= -1;
        }
    }
}
