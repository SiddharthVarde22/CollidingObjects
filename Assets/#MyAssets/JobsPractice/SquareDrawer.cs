using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class SquareDrawer : MonoBehaviour
{
    [SerializeField]
    int m_numberOfObjects = 100;
    [SerializeField]
    Mesh m_meshToDraw;
    [SerializeField]
    Material m_material;
    [SerializeField]
    float m_scale = 0.2f;

    NativeArray<Matrix4x4> m_positionsToDrawAt;
    PracticeJob m_particlesPositionJob;
    JobHandle m_positionsJobHandle;
    RenderParams m_renderParameters;
    //GraphicsBuffer m_commandBuffer;

    void Start()
    {
        m_positionsToDrawAt = new NativeArray<Matrix4x4>(m_numberOfObjects, Allocator.Persistent);
        m_particlesPositionJob = new PracticeJob(m_positionsToDrawAt, m_scale);

        m_renderParameters = new RenderParams(m_material);
        m_renderParameters.worldBounds = new Bounds(Vector3.zero, Vector3.one * 100);
        //m_commandBuffer = new GraphicsBuffer()

    }

    private void OnDestroy()
    {
        m_positionsToDrawAt.Dispose();
    }

    void Update()
    {
        m_positionsJobHandle = m_particlesPositionJob.Schedule<PracticeJob>();
        m_positionsJobHandle.Complete();

        //Graphics.RenderMeshIndirect(m_renderParameters, m_meshToDraw, )
        Graphics.RenderMeshInstanced(m_renderParameters, m_meshToDraw, 0, m_positionsToDrawAt);
    }
}
