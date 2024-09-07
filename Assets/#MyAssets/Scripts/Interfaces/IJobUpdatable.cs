using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJobUpdatable
{
    public bool ShouldUpdate();
    public void OnUpdateCalled(float a_deltaTime);
    public void OnJobCompleteCalled();
}
