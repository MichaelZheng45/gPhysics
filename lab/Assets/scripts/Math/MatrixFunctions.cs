using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatrixFunctions
{
    public static Matrix4x4 getRotationMatrix(Quaternion4D q)
    {
        Matrix4x4 newRotationM = new Matrix4x4(
      new Vector4(1 - (2*(q.y * q.y + q.z*q.z)), 2 * (q.x * q.y + q.w * q.z), 2 * (q.x * q.z - q.w * q.y), 0),
      new Vector4(2 * (q.x * q.y - q.w * q.z), 1 - (2 * (q.x * q.x + q.z * q.z)), 2 * (q.y * q.z + q.w * q.x), 0),
      new Vector4(2 * (q.x * q.z + q.w * q.y), 2 * (q.y * q.z - q.w * q.x), 1 - (2 * (q.y * q.y + q.x * q.x)), 0),
      new Vector4(0, 0, 0, 1));
      return newRotationM;
    }

	public static Matrix4x4 getInverseRotationMatrix(Matrix4x4 rot)
	{
		Vector3 col1 = rot.GetColumn(0);
		Vector3 col2 = rot.GetColumn(1);
		Vector3 col3 = rot.GetColumn(2);
		float a = col1.x;
		float b = col2.x;
		float c = col3.x;
		float d = col1.y;
		float e = col2.y;
		float f = col3.y;
		float g = col1.z;
		float h = col2.z;
		float i = col3.z;

		float det = 1.0f / (a*e*i + d*h*c + g*b*f - a*h*f - g*e*c - d*b*i);
		Matrix4x4 invRot = new Matrix4x4(
			new Vector4(e * i - f * h, f * g - d * i, d * h - e * g, 0) * det,
			new Vector4(c * h - b * i, a * i - c * g, b * g - a * h, 0) * det,
			new Vector4(b * f - c * e, c * d - a * f, a * e - b * d, 0) * det,
			new Vector4(0, 0, 0, 1)
		);

		return invRot;
	}
    public static Matrix4x4 getTransformMatrix(Quaternion4D q, Vector3 position)
    {
        Matrix4x4 newTransform = getRotationMatrix(q);
          
        newTransform.SetColumn(3,new Vector4(position.x, position.y, position.z, 1));

        return newTransform;
    }

    public static Matrix4x4 getTransformInverseMatrix(Quaternion4D q, Vector3 position)
    {
        Matrix4x4 newRotationM = getRotationMatrix(q);
        newRotationM = getInverseRotationMatrix(newRotationM);
        position = (newRotationM) * position * -1;

        Vector4 newPosition = (Vector4)position + new Vector4(0, 0, 0, 1);
        Matrix4x4 newTransform = newRotationM;
        newTransform.SetColumn(3, newPosition);

        return newTransform;
    }
}
