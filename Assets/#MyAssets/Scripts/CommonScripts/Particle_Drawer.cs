using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Drawer : GenericSingleton<Particle_Drawer>, IUpdatable
{
    [SerializeField]
    Mesh m_particleMesh;
    [SerializeField]
    Material m_particleMaterial;
    [SerializeField]
    bool m_shouldUpdate;

    List<Matrix4x4> m_particleMatrices;
    RenderParams m_renderParams;

    public bool ShouldUpdate { get { return m_shouldUpdate; } }

    protected override void Awake()
    {
        base.Awake();
        m_particleMatrices = new List<Matrix4x4>();

        m_renderParams = new RenderParams(m_particleMaterial);
    }

    private void Start()
    {
        UpdateManager.SubscribeForUpdateCall(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UpdateManager.UnsubscribeFromUpdateCall(this);
    }

    public void OnUpdateCalled(float a_deltaTime)
    {
        DrawParticles();
    }

    public static int GetAddedToMtrixList(Matrix4x4 a_particleMatrix)
    {
        int l_currentCount = Instance.m_particleMatrices.Count;
        Instance.m_particleMatrices.Add(a_particleMatrix);
        return l_currentCount;
    }

    public static void UpdateParticleMatrixInList(int a_index, Matrix4x4 a_newParticleMatrix)
    {
        if(Instance.m_particleMatrices.Count > a_index)
        {
            Instance.m_particleMatrices[a_index] = a_newParticleMatrix;
        }
    }

    private void DrawParticles()
    {
        int l_remainingParticles = m_particleMatrices.Count;
        int l_maxLimitTodraw = 1023;
        int l_loops = l_remainingParticles % l_maxLimitTodraw;

        for(int i = 0; i <= l_loops; i++)
        {
            int l_startIndex = i * l_maxLimitTodraw;
            int l_countToDraw = (l_remainingParticles < l_maxLimitTodraw) ? l_remainingParticles : l_maxLimitTodraw;

            Graphics.RenderMeshInstanced(m_renderParams, m_particleMesh, 0, m_particleMatrices, l_countToDraw, l_startIndex);
            l_remainingParticles -= l_countToDraw;
        }

    }
}
