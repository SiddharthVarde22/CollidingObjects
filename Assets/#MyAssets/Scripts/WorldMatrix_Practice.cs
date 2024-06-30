using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMatrix_Practice : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("World matrix is = \n" + transform.localToWorldMatrix);
        }
    }
}
