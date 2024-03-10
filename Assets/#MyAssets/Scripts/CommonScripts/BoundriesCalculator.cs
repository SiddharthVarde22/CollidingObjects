using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundriesCalculator : GenericSingleton<BoundriesCalculator>
{
    Vector2 m_bottomLeftCorner, m_topRightCorner;

    protected override void Awake()
    {
        base.Awake();
        CalculateBoundries();
    }

    private void CalculateBoundries()
    {
        Camera l_mainCamera = Camera.main;
        m_bottomLeftCorner = l_mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 10));
        m_topRightCorner = l_mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 10));
    }

    public static (Vector2, Vector2) GetBounries { get { return (Instance.m_bottomLeftCorner, Instance.m_topRightCorner);} }
}
