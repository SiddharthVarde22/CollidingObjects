using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehavior_3D : ParticleBehavior
{
    [SerializeField]
    Rigidbody m_rigidbody;

    public override void AddForce()
    {
        m_rigidbody.AddForce(new Vector3((Random.Range(-1f, 1f)), (Random.Range(-1f, 1f)), 0), ForceMode.Impulse);
    }
}
