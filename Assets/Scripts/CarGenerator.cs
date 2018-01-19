using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CarGenerator : MonoBehaviour {
    
    public float m_MinX;

    public float m_MaxX;

    public float m_MinY;

    public float m_MaxY;

    public float m_MinZ;

    public float m_MaxZ;

    public float m_WheelMinRadius;

    public float m_WheelMaxRadius;

    public void GenerateCar()
    {
        float randomX = Random.Range(m_MinX, m_MaxX);
        float randomY = Random.Range(m_MinY, m_MaxY);
        float randomZ = Random.Range(m_MinZ, m_MaxZ);
        float randomWheelRadius = Random.Range(m_WheelMinRadius, m_WheelMaxRadius);

        float lowerToUpperProp = Random.Range(.3f, .5f);
        MeshTemplate lowerPart =  GenerateLowerPart(randomX, randomY, randomZ, lowerToUpperProp);
        MeshTemplate upperPart = GenerateUpperPart(randomX, randomY, randomZ, lowerToUpperProp);

        List<MeshTemplate> wheels = new List<MeshTemplate>();
        wheels.Add(MeshTemplatePrimitives.Cylinder(new Vector3(randomX / 2, 0f, randomZ / 2 - 1f), randomWheelRadius, .2f , 12));
        wheels.Add(MeshTemplatePrimitives.Cylinder(new Vector3(randomX / 2, 0f, -randomZ / 2 + .6f), randomWheelRadius, .2f, 12));
        wheels.Add(MeshTemplatePrimitives.Cylinder(new Vector3(-randomX / 2, 0f, randomZ / 2 - 1f), randomWheelRadius, .2f, 12));
        wheels.Add(MeshTemplatePrimitives.Cylinder(new Vector3(-randomX / 2, 0f, -randomZ / 2 + .6f), randomWheelRadius, .2f, 12));

        MeshTemplate car = new MeshTemplate();
        car.Add(lowerPart);
        car.Add(upperPart);

        for (int i = 0; i < wheels.Count; i++)
        {
            car.Add(wheels[i]);       
        }
        GetComponent<MeshFilter>().mesh = car.ToMesh();
    }

    MeshTemplate GenerateLowerPart(float _x, float _y, float _z, float _lowetToUpper)
    {        
        List<Vector3> lowerRing = new List<Vector3>(4);
        List<Vector3> upperRing = new List<Vector3>(4);
     
        lowerRing.Add(new Vector3(_x / 2, 0f, -_z / 2));
        lowerRing.Add(new Vector3(_x / 2, 0f, _z / 2));
        lowerRing.Add(new Vector3(-_x / 2, 0f, _z / 2));
        lowerRing.Add(new Vector3(-_x / 2, 0f, -_z / 2));

        float Y = _y * _lowetToUpper;
        upperRing.Add(new Vector3(_x / 2, Y, -_z / 2));
        upperRing.Add(new Vector3(_x / 2, Y, _z / 2));
        upperRing.Add(new Vector3(-_x / 2, Y, _z / 2));
        upperRing.Add(new Vector3(-_x / 2, Y, -_z / 2));

        MeshTemplate Band = MeshTemplatePrimitives.Band(lowerRing, upperRing);

        MeshTemplate lowerPlane = MeshTemplatePrimitives.Quad(new Vector3(- _x/2, 0f, -_z / 2), new Vector3(_x, 0f, 0f), new Vector3(0f, 0f, _z));
        lowerPlane.FlipFaces();
        
        MeshTemplate UpperPlane = MeshTemplatePrimitives.Quad(new Vector3(-_x / 2, Y, -_z / 2), new Vector3(_x, 0f, 0f), new Vector3(0f, 0f, _z));

        Band.Add(lowerPlane);
        Band.Add(UpperPlane);
        return Band;
    }

    MeshTemplate GenerateUpperPart(float _x, float _y, float _z, float _lowetToUpper)
    {
        List<Vector3> lowerRing = new List<Vector3>(4);
        List<Vector3> upperRing = new List<Vector3>(4);
        float Y =  _y * _lowetToUpper;
      
        float ZFrontLower = Random.Range(.7f * _z, .4f * _z);
        float ZBackLower = Random.Range(.6f * _z, .9f * _z);

        lowerRing.Add(new Vector3(_x / 2, Y, -ZBackLower / 2));
        lowerRing.Add(new Vector3(_x / 2, Y, ZFrontLower / 2));
        lowerRing.Add(new Vector3(-_x / 2, Y, ZFrontLower / 2));
        lowerRing.Add(new Vector3(-_x / 2, Y, -ZBackLower / 2));

        float XUpper = Random.Range(_x, _x * .7f);
        float ZFrontUpper = Random.Range(ZFrontLower, .3f * ZFrontLower);
        float ZBackUpper = Random.Range(ZBackLower, .6f * ZBackLower);

        upperRing.Add(new Vector3(XUpper / 2, _y, -ZBackUpper / 2));
        upperRing.Add(new Vector3(XUpper / 2, _y, ZFrontUpper / 2));
        upperRing.Add(new Vector3(-XUpper / 2, _y, ZFrontUpper / 2));
        upperRing.Add(new Vector3(-XUpper / 2, _y, -ZBackUpper / 2));

        MeshTemplate Band = MeshTemplatePrimitives.Band(lowerRing, upperRing);

        MeshTemplate roof = MeshTemplatePrimitives.Quad(
            new Vector3(-XUpper / 2, _y, -ZBackUpper / 2),
            Vector3.right * XUpper,
            Vector3.forward * (ZFrontUpper / 2 + ZBackUpper / 2));

        Band.Add(roof);

        return Band;
    }
}
