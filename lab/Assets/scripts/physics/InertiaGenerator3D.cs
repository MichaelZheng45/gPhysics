using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InertiaTypes3D
{
    SPHERE,
    HALLOW_SPHERE,
    BOX,
    HALLOW_BOX
}

public class InertiaGenerator3D
{
    public static Matrix4x4 GenerateInertia_Sphere(float mass, float radius)
    {
        Vector4 column1 = new Vector4(.4f * mass * radius * radius, 0, 0, 0);
        Vector4 column2 = new Vector4(0, .4f * mass * radius * radius, 0, 0);
        Vector4 column3 = new Vector4(0, 0, .4f * mass * radius * radius, 0);
        Matrix4x4 inertia = new Matrix4x4(
            column1,
            column2,
            column3,
            new Vector4(0, 0, 0, 1)
            );

        return inertia;
    }

    public static Matrix4x4 GenerateInertia_Hallow_Sphere(float mass, float radius)
    {
        Vector4 column1 = new Vector4(.66f * mass * radius * radius, 0, 0, 0);
        Vector4 column2 = new Vector4(0, .66f * mass * radius * radius, 0, 0);
        Vector4 column3 = new Vector4(0, 0, .66f * mass * radius * radius, 0);
        Matrix4x4 inertia = new Matrix4x4(
            column1,
            column2,
            column3,
                new Vector4(0, 0, 0, 1)
            );

        return inertia;
    }

    public static Matrix4x4 GenerateInertia_Box(float mass, float width, float height, float depth)
    {
        Vector4 column1 = new Vector4(0.0833f *mass *(height * height + depth * depth), 0, 0, 0);
        Vector4 column2 = new Vector4(0, 0.0833f * mass * (width * width + depth * depth), 0, 0);
        Vector4 column3 = new Vector4(0, 0, 0.0833f * mass * (height * height + width * width), 0);
        Matrix4x4 inertia = new Matrix4x4(
            column1,
            column2,
            column3,
               new Vector4(0, 0, 0, 1)
            );

        return inertia;
    }

    public static Matrix4x4 GenerateInertia_Hallow_Box(float mass, float width, float height, float depth)
    {
        Vector4 column1 = new Vector4(1.66f * mass * (height * height + depth * depth), 0, 0, 0);
        Vector4 column2 = new Vector4(1.66f * mass * (width * width + depth * depth), 0, 0);
        Vector4 column3 = new Vector4(0, 0, 1.66f * mass * (height * height + width * width), 0);
        Matrix4x4 inertia = new Matrix4x4(
            column1,
            column2,
            column3,
               new Vector4(0, 0, 0, 1)
            );

        return inertia;
    }
}
