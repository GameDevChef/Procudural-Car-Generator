using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeshTemplatePrimitives
{

    public static MeshTemplate Triangle(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        Vector3 normal = normal = Vector3.Cross((v1 - v0), (v2 - v0)).normalized;
        return new MeshTemplate
        {

            vertices = new List<Vector3>(3) { v0, v1, v2 },
            normals = new List<Vector3>(3) { normal, normal, normal },
            uv = new List<Vector2>(3) { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) },
            triangles = new List<int>(3) { 0, 1, 2 },
            name = "Triangle"
        };
    }

    public static MeshTemplate Quad(Vector3 origin, Vector3 width, Vector3 length)
    {
        Vector3 normal = Vector3.Cross(length, width).normalized;
        return new MeshTemplate
        {
            vertices = new List<Vector3>(4) { origin, origin + length, origin + length + width, origin + width },
            normals = new List<Vector3>(4) { normal, normal, normal, normal },
            uv = new List<Vector2>(4) { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) },
            triangles = new List<int>(6) { 0, 1, 2, 0, 2, 3 },
            name = "Quad"
        };
    }

    public static MeshTemplate Quad(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        var normal = Vector3.Cross((vertex1 - vertex0), (vertex2 - vertex0)).normalized;
        return new MeshTemplate
        {
            vertices = new List<Vector3>(4) { vertex0, vertex1, vertex2, vertex3 },
            normals = new List<Vector3>(4) { normal, normal, normal, normal },
            uv = new List<Vector2>(4) { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) },
            triangles = new List<int>(6) { 0, 1, 2, 0, 2, 3 },
            name = "Quad"
        };
    }

    public static MeshTemplate TriangleStrip(List<Vector3> verticesList)
    {
        var template = new MeshTemplate
        {
            vertices = verticesList,
            triangles = new List<int>(verticesList.Count - 2),
            normals = new List<Vector3>(verticesList.Count),
            uv = new List<Vector2>(verticesList.Count),
            name = "TriangleStrip"
        };
        for (int i = 0, j = 1, k = 2; i < verticesList.Count - 2; i++, j += i % 2 * 2, k += (i + 1) % 2 * 2)
        {
            template.triangles.Add(i);
            template.triangles.Add(j);
            template.triangles.Add(k);

        }
        var normal = Vector3.Cross(verticesList[1] - verticesList[0], verticesList[2] - verticesList[0]).normalized;
        for (int i = 0; i < verticesList.Count; i++)
        {
            template.normals.Add(normal);
            template.uv.Add(new Vector2((float)i / verticesList.Count, (float)i / verticesList.Count));
        }

        return template;
    }

    public static MeshTemplate TriangleFan(List<Vector3> verticesList)
    {
        MeshTemplate template = new MeshTemplate
        {
            vertices = verticesList,
            triangles = new List<int>(verticesList.Count - 2),
            normals = new List<Vector3>(verticesList.Count),
            uv = new List<Vector2>(verticesList.Count),
            name = "Triangle Fan"

        };

        for (int i = 1; i < verticesList.Count - 1; i++)
        {
            template.triangles.Add(0);
            template.triangles.Add(i);
            template.triangles.Add(i + 1);
        }

        var normal = Vector3.Cross(verticesList[1] - verticesList[0], verticesList[2] - verticesList[0]).normalized;

        for (int i = 0; i < verticesList.Count; i++)
        {
            template.normals.Add(normal);
            template.uv.Add(new Vector2((float)i / verticesList.Count, (float)i / verticesList.Count));
        }

        return template;
    }

    public static MeshTemplate BaselessPyramid(Vector3 baseCenter, Vector3 apex, float radius, int segments,
        bool inverted = false)
    {
        float segmentAngle = 360f / segments * (inverted ? -1 : 1);
        float currentAngle = 0f;

        Vector3[] verticesList = new Vector3[segments + 1];
        verticesList[0] = apex;

        for (int i = 1; i <= segments; i++)
        {
            verticesList[i] = Utility.PointOnCircle3XZ(radius, currentAngle) + baseCenter;
            Debug.Log(verticesList[i]);
            currentAngle += segmentAngle;
        }


        MeshTemplate template = new MeshTemplate { name = "Baseless Pyramid" };

        for (int i = 1; i < segments; i++)
        {
            template.Add(Triangle(verticesList[0], verticesList[i], verticesList[i + 1]));
        }
        template.Add(Triangle(verticesList[0], verticesList[verticesList.Length - 1], verticesList[1]));
        Debug.Log(verticesList[0] + " " + verticesList[verticesList.Length - 1] + " " + verticesList[1] + (verticesList.Length - 1));
        return template;
    }

    public static MeshTemplate BaselessPyramid(float radius, int segments, float heignt, bool inverted = false)
    {
        return BaselessPyramid(Vector3.zero, Vector3.up * heignt * (inverted ? -1 : 1), radius, segments, inverted);
    }

    public static MeshTemplate Band(List<Vector3> lowerRing, List<Vector3> upperRing)
    {
        MeshTemplate template = new MeshTemplate { name = "Band" };
        if (lowerRing.Count < 3 || upperRing.Count < 3)
        {
            Debug.LogError("List must be grater then 2");
            return template;
        }
        if (lowerRing.Count != upperRing.Count)
        {
            Debug.LogError("Lists must be equal");
            return template;
        }
        template.vertices.AddRange(lowerRing);
        template.vertices.AddRange(upperRing);

        List<Vector3> lowerNormals = new List<Vector3>();
        List<Vector3> upperNormals = new List<Vector3>();
        var lowerUv = new List<Vector2>();
        var upperUv = new List<Vector2>();

        int i0, i1, i2, i3;
        Vector3 v0, v1, v2, v3;
        for (int i = 0; i < lowerRing.Count - 1; i++)
        {
            i0 = i;
            i1 = i + lowerRing.Count;
            i2 = i + 1;
            i3 = i + 1 + lowerRing.Count;
            v0 = template.vertices[i0];
            v1 = template.vertices[i1];
            v2 = template.vertices[i2];
            v3 = template.vertices[i3];
            template.triangles.AddRange(new[] { i0, i1, i2 });
            template.triangles.AddRange(new[] { i2, i1, i3 });

            lowerNormals.Add(Vector3.Cross(v1 - v0, v2 - v0).normalized);
            upperNormals.Add(Vector3.Cross(v3 - v1, v0 - v1).normalized);

            var u = (float)i / (lowerRing.Count - 1);
            lowerUv.Add(new Vector2(u, 0));
            upperUv.Add(new Vector2(u, 1));
        }
        i0 = lowerRing.Count - 1;
        i1 = lowerRing.Count * 2 - 1;
        i2 = 0;
        i3 = lowerRing.Count;
        v0 = template.vertices[i0];
        v1 = template.vertices[i1];
        v2 = template.vertices[i2];
        v3 = template.vertices[i3];
        template.triangles.AddRange(new[] { i0, i1, i2 });
        template.triangles.AddRange(new[] { i2, i1, i3 });

        lowerNormals.Add(Vector3.Cross(v1 - v0, v2 - v0).normalized);
        upperNormals.Add(Vector3.Cross(v3 - v1, v0 - v1).normalized);
        template.normals.AddRange(lowerNormals);
        template.normals.AddRange(upperNormals);

        lowerUv.Add(new Vector2(1, 0));
        upperUv.Add(new Vector2(1, 1));
        template.uv.AddRange(lowerUv);
        template.uv.AddRange(upperUv);

        return template;
    }

    public static MeshTemplate FlatBand(List<Vector3> lowerRing, List<Vector3> upperRing)
    {
        var draft = new MeshTemplate { name = "Flat band" };
        if (lowerRing.Count < 3 || upperRing.Count < 3)
        {
            Debug.LogError("Array sizes must be greater than 2");
            return draft;
        }
        if (lowerRing.Count != upperRing.Count)
        {
            Debug.LogError("Array sizes must be equal");
            return draft;
        }

        Vector3 v0, v1, v2, v3;
        for (int i = 0; i < lowerRing.Count - 1; i++)
        {
            v0 = lowerRing[i];
            v1 = upperRing[i];
            v2 = lowerRing[i + 1];
            v3 = upperRing[i + 1];
            draft.Add(Triangle(v0, v1, v2));
            draft.Add(Triangle(v2, v1, v3));
        }

        v0 = lowerRing[lowerRing.Count - 1];
        v1 = upperRing[upperRing.Count - 1];
        v2 = lowerRing[0];
        v3 = upperRing[0];
        draft.Add(Triangle(v0, v1, v2));
        draft.Add(Triangle(v2, v1, v3));

        return draft;
    }

    public static MeshTemplate Pyramid(float radius, int segments, float heignt, bool inverted = false)
    {
        var draft = BaselessPyramid(radius, segments, heignt, inverted);
        var vertices = new List<Vector3>(segments);
        for (int i = draft.vertices.Count - 2; i >= 0; i -= 3)
        {
            vertices.Add(draft.vertices[i]);
        }
        draft.Add(TriangleFan(vertices));
        draft.name = "Pyramid";
        return draft;
    }

    public static MeshTemplate Cylinder(Vector3 baseCenter, float radius, float height, int segments, bool inverted = false)
    {
        {
            float segmentAngle = 360f / segments;
            float currentAngle = 0;

            var lowerRing = new List<Vector3>(segments);
            var upperRing = new List<Vector3>(segments);
            for (var i = 0; i < segments; i++)
            {
                var point = Utility.PointOnCircle3YZ(radius, currentAngle);
                lowerRing.Add(point + Vector3.right * height / 2 + baseCenter);
                upperRing.Add(point - Vector3.right * height / 2 + baseCenter);
                currentAngle -= segmentAngle;
            }

            var draft = TriangleFan(lowerRing);
            draft.Add(Band(lowerRing, upperRing));
            upperRing.Reverse();
            draft.Add(TriangleFan(upperRing));
            draft.name = "Cylinder";
            return draft;
        }



    }

 
}



