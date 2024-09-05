using Unity.Jobs;
using Unity.Collections;
using UnityEngine;
using Unity.Mathematics;

public struct PracticeJob : IJob
    {
    public NativeArray<Matrix4x4> m_positions;
    float m_size;

    public PracticeJob(NativeArray<Matrix4x4> a_matrix, float a_size)
    {
        m_positions = a_matrix;
        m_size = a_size;
    }

    public void Execute()
    {
        Unity.Mathematics.Random l_random = new Unity.Mathematics.Random(5);
        int l_length = m_positions.Length;
        Vector3 l_randomPosition = Vector3.zero;

        for (int i = 0; i < l_length; i++)
        {
            l_randomPosition.x = l_random.NextFloat(-5, 5);
            l_randomPosition.y = l_random.NextFloat(-5, 5);

            m_positions[i] = Matrix4x4.TRS(l_randomPosition, Quaternion.identity, Vector3.one * m_size);
        }

        //l_randomPosition.x = l_random.NextFloat(-5, 5);
        //l_randomPosition.y = l_random.NextFloat(-5, 5);
        //m_positions[a_index] = Matrix4x4.TRS(l_randomPosition, Quaternion.identity, Vector3.one * m_size);
    }
}
