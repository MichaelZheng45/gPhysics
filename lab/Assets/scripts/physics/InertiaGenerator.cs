using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InertiaTypes
{
    CIRCLE,
    RING,
    RECTANGE,
    ROD
}

//https://en.wikipedia.org/wiki/List_of_second_moments_of_area
public class InertiaGenerator
{
    public static float GenerateInertia_Circle(float mass, float radius)
    {
        float inertia = (mass * radius * radius) * 0.5f;
        return inertia;
    }

    public static float GenerateInertia_Ring(float mass, float outerRadius, float innerRadius)
    {
        float inertia = mass * ((outerRadius * outerRadius) + (innerRadius * innerRadius)) * 0.5f;
        return inertia;
    }

    public static float GenerateInertia_Rectangle(float mass, float width, float height)
    {
        float inertia = mass * ((width * width) + (height * height)) / 12f;
        return inertia;
    }

    public static float GenerateInertia_Ellipse(float mass, float length)
    {
        float inertia = (mass * length * length) / 12f;
        
        return inertia;

    }
    
}
