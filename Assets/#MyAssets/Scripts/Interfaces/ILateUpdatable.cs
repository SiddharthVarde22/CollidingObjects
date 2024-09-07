using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILateUpdatable
{
    public bool ShouldUpdate();
    public void OnLateUpdateCalled(float a_deltaTime);
}
