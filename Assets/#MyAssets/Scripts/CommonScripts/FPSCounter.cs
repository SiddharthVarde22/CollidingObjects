using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : GenericSingleton<FPSCounter>, IUpdatable
{
    [SerializeField]
    private bool m_shouldUpdate = true;
    [SerializeField]
    private TextMeshProUGUI m_fpsText;

    private float m_timePassed = 0;
    private float m_framesPassed = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        UpdateManager.SubscribeForUpdateCall(this);
    }

    protected override void OnDestroy()
    {
        UpdateManager.UnsubscribeFromUpdateCall(this);
        base.OnDestroy();
    }

    public bool ShouldUpdate { get => m_shouldUpdate; }

    public void OnUpdateCalled(float a_deltaTime)
    {
        m_timePassed += a_deltaTime;
        m_framesPassed++;

        if (m_timePassed >= 1)
        {
            m_fpsText.text = "FPS : " + m_framesPassed;
            m_timePassed = 0;
            m_framesPassed = 0;
        }
    }
}
