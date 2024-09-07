using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpaceable
{
    public Vector3 GetPosition();
    public Vector3 GetSpeedDirection();
    public float GetScale();
    public void SetSpeedDirection(Vector3 a_speedDirection);
}
