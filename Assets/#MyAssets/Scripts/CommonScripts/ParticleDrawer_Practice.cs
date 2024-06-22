using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDrawer_Practice : MonoBehaviour
{
    [SerializeField]
    Mesh m_particleMeshToDraw;
    [SerializeField]
    Material m_particleMaterial;

    [SerializeField]
    int m_numberOfParticlesToSpawn = 600;

    RenderParams m_renderParams;
    //Matrix4x4 m_worldMatrix;
    Matrix4x4[] m_matrices;


    private void Start()
    {
        m_renderParams = new RenderParams(m_particleMaterial);
        m_matrices = new Matrix4x4[m_numberOfParticlesToSpawn];
        transform.localScale = Vector3.one * 0.2f;
        //transform.position = Vector3.one;
        //Debug.LogError(transform.localToWorldMatrix);
        //transform.position = 2 * Vector3.one;
        //Debug.LogError(transform.localToWorldMatrix);
        //transform.position = Vector3.zero;
        //Debug.LogError(transform.localToWorldMatrix);
    }

    void Update()
    {
        //m_worldMatrix = transform.localToWorldMatrix;

        //Graphics.RenderMesh(m_renderParams, m_particleMeshToDraw, 0, m_worldMatrix);

        for(int i = 0; i < m_numberOfParticlesToSpawn; i++)
        {
            transform.position += (transform.right * 0.25f);
            m_matrices[i] = transform.localToWorldMatrix;
        }

        transform.position = Vector3.zero;
        Graphics.RenderMeshInstanced(m_renderParams, m_particleMeshToDraw, 0, m_matrices);
    }
}
