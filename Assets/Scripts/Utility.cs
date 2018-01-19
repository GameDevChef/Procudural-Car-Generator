using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{

    public static Vector3 PointOnCircle3XY(float _radius, float _angle)
    {
        float angleInRadians = _angle * Mathf.Deg2Rad;
        return new Vector3(_radius * Mathf.Sin(angleInRadians), _radius * Mathf.Cos(angleInRadians), 0);
    }

    public static Vector3 PointOnCircle3XZ(float _radius, float _angle)
    {
        float angleInRadians = _angle * Mathf.Deg2Rad;
        return new Vector3(_radius * Mathf.Sin(angleInRadians), 0, _radius * Mathf.Cos(angleInRadians));
    }

    public static Vector3 PointOnCircle3YZ(float _radius, float _angle)
    {
        float angleInRadians = _angle * Mathf.Deg2Rad;
        return new Vector3(0, _radius * Mathf.Sin(angleInRadians), _radius * Mathf.Cos(angleInRadians));
    }

    public static void Swap<T>(ref T _left, ref T _right)
    {
        T temp = _left;
        _left = _right;
        _right = temp;
    }

  
}
