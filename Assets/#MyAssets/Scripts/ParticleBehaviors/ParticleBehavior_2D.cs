using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehavior_2D : ParticleBehavior
{
    [SerializeField]
    Rigidbody2D m_rigidbody;

    public override void AddForce()
    {
        m_rigidbody.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode2D.Impulse);
    }
}
