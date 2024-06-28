
using UnityEngine;

public class Particle_Drawer : GenericSingleton<Particle_Drawer>, IUpdatable
{
    [SerializeField]
    Mesh m_particleMesh;
    [SerializeField]
    Material m_particleMaterial;
    [SerializeField]
    bool m_shouldUpdate;

    //List<Matrix4x4> m_particleMatrices;
    Matrix4x4[] m_particleMatrices;
    int m_numberOfMatrixesInArray;
    RenderParams m_renderParams;

    public bool ShouldUpdate { get { return m_shouldUpdate; } }

    protected override void Awake()
    {
        base.Awake();

        m_renderParams = new RenderParams(m_particleMaterial);
        m_renderParams.worldBounds = new Bounds(Vector3.zero, Vector3.one * 100);
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

    public void OnUpdateCalled(float a_deltaTime)
    {
        DrawParticles();
    }

    public static int GetAddedToMtrixList(Matrix4x4 a_particleMatrix)
    {
        int l_currentCount = Instance.m_particleMatrices.Length;
        Instance.m_particleMatrices[l_currentCount] = a_particleMatrix;
        return l_currentCount;
    }

    public static void UpdateParticleMatrixInList(int a_index, Matrix4x4 a_newParticleMatrix)
    {
        Instance.m_particleMatrices[a_index] = a_newParticleMatrix;

        //if (Instance.m_particleMatrices.Count > a_index)
        //{
        //}
    }

    public static void UpdateParticleMatrixInList(int a_index, Vector4 a_position)
    {
        a_position.w = 1;
        Instance.m_particleMatrices[a_index].SetColumn(3, a_position);
    }

    private void DrawParticles()
    {
        int l_remainingParticles = m_particleMatrices.Length;
        int l_maxLimitTodraw = 1023;
        //int l_loops = l_remainingParticles % l_maxLimitTodraw;

        //Debug.Log("Loops = " + l_loops + " remainimg particles = " + l_remainingParticles + " max Limit = " + l_maxLimitTodraw);
        for (int i = 0; l_remainingParticles > 0 ; i++)
        {
            int l_startIndex = i * l_maxLimitTodraw;
            int l_countToDraw = (l_remainingParticles < l_maxLimitTodraw) ? l_remainingParticles : l_maxLimitTodraw;
            //Debug.Log("Count to draw = " + l_countToDraw + "Start index = " + l_startIndex);
            Graphics.RenderMeshInstanced(m_renderParams, m_particleMesh, 0, m_particleMatrices, l_countToDraw, l_startIndex);

            l_remainingParticles -= l_countToDraw;
        }
    }

    public static void InitializeList(int a_maxParticleCount)
    {
        //Instance.m_particleMatrices = new List<Matrix4x4>(a_maxParticleCount);
        Instance.m_particleMatrices = new Matrix4x4[a_maxParticleCount];
        Instance.m_numberOfMatrixesInArray = 0;
    }

    public static void AddRangeOfParticles(Matrix4x4[] a_spawnedParticles)
    {
        //Instance.m_particleMatrices.AddRange(a_spawnedParticles);
        for(int i = 0, l_length = Instance.m_numberOfMatrixesInArray; i < a_spawnedParticles.Length; i++)
        {
            //Debug.Log("Length = " + l_length + " i is = " + i + " incoming array length " + a_spawnedParticles.Length);
            Instance.m_particleMatrices[l_length + i] = a_spawnedParticles[i];
            Instance.m_numberOfMatrixesInArray++;
        }
    }
}
