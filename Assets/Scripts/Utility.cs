using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{

    public static Vector3 PointOnCircle3XY(float radius, float angle)
    {
        float angleInRadians = angle * Mathf.Deg2Rad;
        return new Vector3(radius * Mathf.Sin(angleInRadians), radius * Mathf.Cos(angleInRadians), 0);
    }

    public static Vector3 PointOnCircle3XZ(float radius, float angle)
    {
        float angleInRadians = angle * Mathf.Deg2Rad;
        return new Vector3(radius * Mathf.Sin(angleInRadians), 0, radius * Mathf.Cos(angleInRadians));
    }

    public static Vector3 PointOnCircle3YZ(float radius, float angle)
    {
        float angleInRadians = angle * Mathf.Deg2Rad;
        return new Vector3(0, radius * Mathf.Sin(angleInRadians), radius * Mathf.Cos(angleInRadians));
    }

    public static void Swap<T>(ref T left, ref T right)
    {
        T temp = left;
        left = right;
        right = temp;
    }

  
}
