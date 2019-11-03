using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatrixFunctions
{
    public static Matrix4x4 getRotationMatrix(Quaternion4D q)
    {
        Matrix4x4 newRotationM = new Matrix4x4(
      new Vector4((q.w * q.w + q.x * q.x - q.y * q.y - q.z * q.z), 2 * (q.x * q.y + q.w * q.z), 2 * (q.x * q.z - q.w * q.y), 0),
      new Vector4(2 * (q.x * q.y - q.w * q.z), (q.w * q.w - q.x * q.x + q.y * q.y - q.z * q.z), 2 * (q.y * q.z + q.w * q.x), 0),
      new Vector4(2 * (q.x * q.z + q.w * q.y), 2 * (q.y * q.z - q.w * q.x), (q.w * q.w - q.x * q.x - q.y * q.y + q.z * q.z), 0),
      new Vector4(0, 0, 0, 0));
      return newRotationM;
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
        newRotationM = newRotationM.inverse;
        position = newRotationM * position;

        Vector4 newPosition = (Vector4)position + new Vector4(0, 0, 0, 1);
        Matrix4x4 newTransform = newRotationM;
        newTransform.SetColumn(3, newPosition);

        return newTransform;
    }
}
