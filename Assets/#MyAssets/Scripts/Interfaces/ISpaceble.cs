using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpaceble
{
    public Vector3 GetPosition();
    public Vector3 GetDirection();
    public void SetDirection(Vector3 a_direction);
    public float GetSize();
}
