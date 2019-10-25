using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quaternion4D 
{
    public float w; 
    public float x;
    public float y;
    public float z;

    public Quaternion4D(float w, float x, float y, float z)
    {
        this.w = w;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Quaternion4D() { }

    public Quaternion4D(float rotation, Vector3 axis)
    {
        setQuaternion(rotation, axis);
    }

    public Vector3 getAxis()
    {
        Vector3 vec0 = new Vector3(x, y, z);
        Vector3 vec1 = new Vector3(Mathf.Abs(x), Mathf.Abs(y), Mathf.Abs(z));
        return new Vector3(x, y, z) * 1.0f / (Mathf.Sin(getAngle() * 0.5f * Mathf.Deg2Rad));
    }


    public float getAngle()
    {
        return 2 * Mathf.Acos(w) * Mathf.Rad2Deg;
    }


    public Vector4 getQuaternionInVector()
    {
        return new Vector4(x, y, z, w);
    }

    public void setQuaternion(float rotation, Vector3 axis)
    {
        float toRad = Mathf.Deg2Rad;
        w = Mathf.Cos(rotation * 0.5f);
        x = axis.x * Mathf.Sin(rotation * 0.5f * toRad);
        y = axis.y * Mathf.Sin(rotation * 0.5f * toRad);
        z = axis.z * Mathf.Sin(rotation * 0.5f * toRad);
    }


    public static Quaternion4D quaternionMult(Quaternion4D firstQ, Quaternion4D secondQ)
    {
        Quaternion4D newQ = new Quaternion4D();
        newQ.w = firstQ.w * secondQ.w - firstQ.x * secondQ.x - firstQ.y * secondQ.y - firstQ.z * secondQ.z;
        newQ.x = firstQ.w * secondQ.x + firstQ.x * secondQ.w - firstQ.y * secondQ.z - firstQ.z * secondQ.y;
        newQ.y = firstQ.w * secondQ.y - firstQ.x * secondQ.z + firstQ.y * secondQ.w - firstQ.z * secondQ.x;
        newQ.z = firstQ.w * secondQ.z + firstQ.x * secondQ.y - firstQ.y * secondQ.x + firstQ.z * secondQ.w;
        return newQ;
    }

    public static Quaternion4D normalize(Quaternion4D q)
    {
        float d = q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z;
        if (d == 0)
        {
            q.w = 1;
            return q;
        }

        d = (1.0f) / Mathf.Sqrt(d);
        q.w *= d;
        q.x *= d;
        q.y *= d;
        q.z *= d;

        return q;
    }

    public static Quaternion4D addScaledVector(Quaternion4D quaternion ,Vector3 v, float scale)
    {
        Quaternion4D q = new Quaternion4D();
        q.w = 0;
        q.x = v.x * scale;
        q.y = v.y * scale;
        q.z = v.z * scale;
        q = quaternionMult(q, quaternion);

        q.w = quaternion.w + q.w * .5f;
        q.x = quaternion.x + q.x * .5f;
        q.y = quaternion.y + q.y * .5f;
        q.z = quaternion.z + q.y * .5f;

        return q;
    }

    public static Quaternion4D operator *(Quaternion4D a, float b)
        => new Quaternion4D(a.w * b, a.x * b, a.y * b, a.z * b);

    public static Vector3 operator *(Quaternion4D a, Vector3 b)
    {
        //From https://answers.unity.com/questions/372371/multiply-quaternion-by-vector3-how-is-done.html
        float num = a.x * 2f;
        float num2 = a.y * 2f;
        float num3 = a.z * 2f;
        float num4 = a.x * num;
        float num5 = a.y * num2;
        float num6 = a.z * num3;
        float num7 = a.x * num2;
        float num8 = a.x * num3;
        float num9 = a.y * num3;
        float num10 = a.w * num;
        float num11 = a.w * num2;
        float num12 = a.w * num3;
        Vector3 result;
        result.x = (1f - (num5 + num6)) * b.x + (num7 - num12) * b.y + (num8 + num11) * b.z;
        result.y = (num7 + num12) * b.x + (1f - (num4 + num6)) * b.y + (num9 - num10) * b.z;
        result.z = (num8 - num11) * b.x + (num9 + num10) * b.y + (1f - (num4 + num5)) * b.z;
        return result;
    }
}
