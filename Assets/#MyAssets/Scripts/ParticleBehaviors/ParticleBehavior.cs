using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParticleBehavior : MonoBehaviour
{
    public abstract void AddForce();

    protected virtual void Start()
    {
        AddForce();
    }
}
