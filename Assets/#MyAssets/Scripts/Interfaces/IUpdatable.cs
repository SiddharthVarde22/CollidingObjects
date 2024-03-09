using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdatable
{
    public bool ShouldUpdate { get; }
    public void OnUpdateCalled(float a_deltaTime);
}
