using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InertiaTypes3D
{
    SPHERE,
    TORUS,
    BOX,
    ROD
}

public class InertiaGenerator3D
{
   public static float GenerateInertia_Shere(float mass, float radius)
    {
        float inertia = (mass * radius * radius) * 0.5f;

        return inertia;
    }

    public static float GenerateInertia_Torus(float mass, float outerRadius, float innerRadius)
    {
        return 0;
    }

    public static float GenerateInertia_Box(float mass, float width, float height, float length)
    {
        return 0;
    }

    public static float GenerateInertia_Rod(float mass, float length)
    {
        return 0;
    }
}
