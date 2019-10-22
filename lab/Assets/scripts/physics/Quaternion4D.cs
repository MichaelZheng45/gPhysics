using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quaternion4D 
{
    float w; 
    float x;
    float y;
    float z;

    public void setQuaternion(float rotation, Vector3 axis)
    {
        w = Mathf.Cos(rotation * 0.5f);
        x = Mathf.Sin(rotation * 0.5f);
        y = Mathf.Sin(rotation * 0.5f);
        z = Mathf.Sin(rotation * 0.5f);
    }
}
