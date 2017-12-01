using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarGenerator : MonoBehaviour {

    
    public float minX;
    public float maxX;

    public float minY;
    public float maxY;

    public float minZ;
    public float maxZ;



    public void GenerateCar()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        float randomZ = Random.Range(minZ, maxZ);
        float lowerToUpperProp = Random.Range(.3f, .5f);
        MeshTemplate lowerPart =  GenerateLowerPart(randomX, randomY, randomZ, lowerToUpperProp);
        MeshTemplate upperPart = GenerateUpperPart(randomX, randomY, randomZ, lowerToUpperProp);
        List<MeshTemplate> wheels = new List<MeshTemplate>();
        wheels.Add(MeshTemplatePrimitives.Cylinder(new Vector3(randomX / 2, 0f, randomZ / 2 - 1f), .4f, .2f , 12));
        wheels.Add(MeshTemplatePrimitives.Cylinder(new Vector3(randomX / 2, 0f, -randomZ / 2 + .6f), .4f, .2f, 12));
        wheels.Add(MeshTemplatePrimitives.Cylinder(new Vector3(-randomX / 2, 0f, randomZ / 2 - 1f), .4f, .2f, 12));
        wheels.Add(MeshTemplatePrimitives.Cylinder(new Vector3(-randomX / 2, 0f, -randomZ / 2 + .6f), .4f, .2f, 12));

        MeshTemplate car = new MeshTemplate();

        car.Add(lowerPart);
        car.Add(upperPart);
        for (int i = 0; i < wheels.Count; i++)
        {
            car.Add(wheels[i]);
            Debug.Log("wheel");
        }

        GetComponent<MeshFilter>().mesh = car.ToMesh();
    }

    private MeshTemplate GenerateLowerPart(float x, float y, float z, float loweToUpper)
    {
        

        List<Vector3> lowerRing = new List<Vector3>(4);
        List<Vector3> upperRing = new List<Vector3>(4);
     
        lowerRing.Add(new Vector3(x / 2, 0f, -z / 2));
        lowerRing.Add(new Vector3(x / 2, 0f, z / 2));
        lowerRing.Add(new Vector3(-x / 2, 0f, z / 2));
        lowerRing.Add(new Vector3(-x / 2, 0f, -z / 2));

        float Y = y * loweToUpper;
        upperRing.Add(new Vector3(x / 2, Y, -z / 2));
        upperRing.Add(new Vector3(x / 2, Y, z / 2));
        upperRing.Add(new Vector3(-x / 2, Y, z / 2));
        upperRing.Add(new Vector3(-x / 2, Y, -z / 2));

        MeshTemplate Band = MeshTemplatePrimitives.Band(lowerRing, upperRing);

        MeshTemplate lowerPlane = MeshTemplatePrimitives.Quad(new Vector3(- x/2, 0f, -z / 2), new Vector3(x, 0f, 0f), new Vector3(0f, 0f, z));
        lowerPlane.FlipFaces();
        
        MeshTemplate UpperPlane = MeshTemplatePrimitives.Quad(new Vector3(-x / 2, Y, -z / 2), new Vector3(x, 0f, 0f), new Vector3(0f, 0f, z));

        Band.Add(lowerPlane);
        Band.Add(UpperPlane);
        return Band;
    }

    private MeshTemplate GenerateUpperPart(float x, float y, float z, float loweToUpper)
    {


        List<Vector3> lowerRing = new List<Vector3>(4);
        List<Vector3> upperRing = new List<Vector3>(4);
        float Y =  y * loweToUpper;

       
        float ZFrontLower = Random.Range(.7f * z, .4f * z);
        float ZBackLower = Random.Range(.6f * z, .9f * z);

        lowerRing.Add(new Vector3(x / 2, Y, -ZBackLower / 2));
        lowerRing.Add(new Vector3(x / 2, Y, ZFrontLower / 2));
        lowerRing.Add(new Vector3(-x / 2, Y, ZFrontLower / 2));
        lowerRing.Add(new Vector3(-x / 2, Y, -ZBackLower / 2));

        float XUpper = Random.Range(x, x * .7f);
        float ZFrontUpper = Random.Range(ZFrontLower, .3f * ZFrontLower);
        float ZBackUpper = Random.Range(ZBackLower, .6f * ZBackLower);

        upperRing.Add(new Vector3(XUpper / 2, y, -ZBackUpper / 2));
        upperRing.Add(new Vector3(XUpper / 2, y, ZFrontUpper / 2));
        upperRing.Add(new Vector3(-XUpper / 2, y, ZFrontUpper / 2));
        upperRing.Add(new Vector3(-XUpper / 2, y, -ZBackUpper / 2));

        MeshTemplate Band = MeshTemplatePrimitives.Band(lowerRing, upperRing);

        MeshTemplate roof = MeshTemplatePrimitives.Quad(
            new Vector3(-XUpper / 2, y, -ZBackUpper / 2),
            Vector3.right * XUpper,
            Vector3.forward * (ZFrontUpper / 2 + ZBackUpper / 2));

        Band.Add(roof);

        return Band;

    }


}
