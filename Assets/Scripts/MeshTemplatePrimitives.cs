using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeshTemplatePrimitives
{

    public static MeshTemplate Triangle(Vector3 _v0, Vector3 _v1, Vector3 _v2)
    {
        Vector3 normal = Vector3.Cross((_v1 - _v0), (_v2 - _v0)).normalized;
        return new MeshTemplate
        {
            m_Vertices = new List<Vector3>(3) { _v0, _v1, _v2 },
            m_Normals = new List<Vector3>(3) { normal, normal, normal },
            m_UVs = new List<Vector2>(3) { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) },
            m_Triangles = new List<int>(3) { 0, 1, 2 },
            m_Name = "Triangle"
        };
    }

    public static MeshTemplate Quad(Vector3 _origin, Vector3 _width, Vector3 _length)
    {
        Vector3 normal = Vector3.Cross(_length, _width).normalized;
        return new MeshTemplate
        {
            m_Vertices = new List<Vector3>(4) { _origin, _origin + _length, _origin + _length + _width, _origin + _width },
            m_Normals = new List<Vector3>(4) { normal, normal, normal, normal },
            m_UVs = new List<Vector2>(4) { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) },
            m_Triangles = new List<int>(6) { 0, 1, 2, 0, 2, 3 },
            m_Name = "Quad"
        };
    }

    public static MeshTemplate Quad(Vector3 _vertex0, Vector3 _vertex1, Vector3 _vertex2, Vector3 _vertex3)
    {
        Vector3 normal = Vector3.Cross((_vertex1 - _vertex0), (_vertex2 - _vertex0)).normalized;
        return new MeshTemplate
        {
            m_Vertices = new List<Vector3>(4) { _vertex0, _vertex1, _vertex2, _vertex3 },
            m_Normals = new List<Vector3>(4) { normal, normal, normal, normal },
            m_UVs = new List<Vector2>(4) { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) },
            m_Triangles = new List<int>(6) { 0, 1, 2, 0, 2, 3 },
            m_Name = "Quad"
        };
    }

    public static MeshTemplate TriangleStrip(List<Vector3> _verticesList)
    {
        MeshTemplate template = new MeshTemplate
        {
            m_Vertices = _verticesList,
            m_Triangles = new List<int>(_verticesList.Count - 2),
            m_Normals = new List<Vector3>(_verticesList.Count),
            m_UVs = new List<Vector2>(_verticesList.Count),
            m_Name = "TriangleStrip"
        };

        for (int i = 0, j = 1, k = 2; i < _verticesList.Count - 2; i++, j += i % 2 * 2, k += (i + 1) % 2 * 2)
        {
            template.m_Triangles.Add(i);
            template.m_Triangles.Add(j);
            template.m_Triangles.Add(k);
        }

        Vector3 normal = Vector3.Cross(_verticesList[1] - _verticesList[0], _verticesList[2] - _verticesList[0]).normalized;
        for (int i = 0; i < _verticesList.Count; i++)
        {
            template.m_Normals.Add(normal);
            template.m_UVs.Add(new Vector2((float)i / _verticesList.Count, (float)i / _verticesList.Count));
        }

        return template;
    }

    public static MeshTemplate TriangleFan(List<Vector3> _verticesList)
    {
        MeshTemplate template = new MeshTemplate
        {
            m_Vertices = _verticesList,
            m_Triangles = new List<int>(_verticesList.Count - 2),
            m_Normals = new List<Vector3>(_verticesList.Count),
            m_UVs = new List<Vector2>(_verticesList.Count),
            m_Name = "Triangle Fan"

        };

        for (int i = 1; i < _verticesList.Count - 1; i++)
        {
            template.m_Triangles.Add(0);
            template.m_Triangles.Add(i);
            template.m_Triangles.Add(i + 1);
        }

        Vector3 normal = Vector3.Cross(_verticesList[1] - _verticesList[0], _verticesList[2] - _verticesList[0]).normalized;

        for (int i = 0; i < _verticesList.Count; i++)
        {
            template.m_Normals.Add(normal);
            template.m_UVs.Add(new Vector2((float)i / _verticesList.Count, (float)i / _verticesList.Count));
        }

        return template;
    }

    public static MeshTemplate BaselessPyramid(Vector3 _baseCenter, Vector3 _apex, float _radius, int _segments, bool _inverted = false)
    {
        float segmentAngle = 360f / _segments * (_inverted ? -1 : 1);
        float currentAngle = 0f;

        Vector3[] verticesList = new Vector3[_segments + 1];
        verticesList[0] = _apex;

        for (int i = 1; i <= _segments; i++)
        {
            verticesList[i] = Utility.PointOnCircle3XZ(_radius, currentAngle) + _baseCenter;
            Debug.Log(verticesList[i]);
            currentAngle += segmentAngle;
        }


        MeshTemplate template = new MeshTemplate { m_Name = "Baseless Pyramid" };

        for (int i = 1; i < _segments; i++)
        {
            template.Add(Triangle(verticesList[0], verticesList[i], verticesList[i + 1]));
        }
        template.Add(Triangle(verticesList[0], verticesList[verticesList.Length - 1], verticesList[1]));
        Debug.Log(verticesList[0] + " " + verticesList[verticesList.Length - 1] + " " + verticesList[1] + (verticesList.Length - 1));
        return template;
    }

    public static MeshTemplate BaselessPyramid(float _radius, int _segments, float _heignt, bool _inverted = false)
    {
        return BaselessPyramid(Vector3.zero, Vector3.up * _heignt * (_inverted ? -1 : 1), _radius, _segments, _inverted);
    }

    public static MeshTemplate Band(List<Vector3> _lowerRing, List<Vector3> _upperRing)
    {
        MeshTemplate template = new MeshTemplate { m_Name = "Band" };
        if (_lowerRing.Count < 3 || _upperRing.Count < 3)
        {
            Debug.Log("List must be grater then 2");
            return template;
        }
        if (_lowerRing.Count != _upperRing.Count)
        {
            Debug.Log("Lists must be equal");
            return template;
        }
        template.m_Vertices.AddRange(_lowerRing);
        template.m_Vertices.AddRange(_upperRing);

        List<Vector3> lowerNormals = new List<Vector3>();
        List<Vector3> upperNormals = new List<Vector3>();
        List<Vector2> lowerUv = new List<Vector2>();
        List<Vector2> upperUv = new List<Vector2>();

        int i0, i1, i2, i3;
        Vector3 v0, v1, v2, v3;
        for (int i = 0; i < _lowerRing.Count - 1; i++)
        {
            i0 = i;
            i1 = i + _lowerRing.Count;
            i2 = i + 1;
            i3 = i + 1 + _lowerRing.Count;
            v0 = template.m_Vertices[i0];
            v1 = template.m_Vertices[i1];
            v2 = template.m_Vertices[i2];
            v3 = template.m_Vertices[i3];
            template.m_Triangles.AddRange(new[] { i0, i1, i2 });
            template.m_Triangles.AddRange(new[] { i2, i1, i3 });

            lowerNormals.Add(Vector3.Cross(v1 - v0, v2 - v0).normalized);
            upperNormals.Add(Vector3.Cross(v3 - v1, v0 - v1).normalized);

            float u = (float)i / (_lowerRing.Count - 1);
            lowerUv.Add(new Vector2(u, 0));
            upperUv.Add(new Vector2(u, 1));
        }
        i0 = _lowerRing.Count - 1;
        i1 = _lowerRing.Count * 2 - 1;
        i2 = 0;
        i3 = _lowerRing.Count;
        v0 = template.m_Vertices[i0];
        v1 = template.m_Vertices[i1];
        v2 = template.m_Vertices[i2];
        v3 = template.m_Vertices[i3];
        template.m_Triangles.AddRange(new[] { i0, i1, i2 });
        template.m_Triangles.AddRange(new[] { i2, i1, i3 });

        lowerNormals.Add(Vector3.Cross(v1 - v0, v2 - v0).normalized);
        upperNormals.Add(Vector3.Cross(v3 - v1, v0 - v1).normalized);
        template.m_Normals.AddRange(lowerNormals);
        template.m_Normals.AddRange(upperNormals);

        lowerUv.Add(new Vector2(1, 0));
        upperUv.Add(new Vector2(1, 1));
        template.m_UVs.AddRange(lowerUv);
        template.m_UVs.AddRange(upperUv);

        return template;
    }

    public static MeshTemplate FlatBand(List<Vector3> _lowerRing, List<Vector3> _upperRing)
    {
        MeshTemplate draft = new MeshTemplate { m_Name = "Flat band" };
        if (_lowerRing.Count < 3 || _upperRing.Count < 3)
        {
            Debug.LogError("Array sizes must be greater than 2");
            return draft;
        }
        if (_lowerRing.Count != _upperRing.Count)
        {
            Debug.LogError("Array sizes must be equal");
            return draft;
        }

        Vector3 v0, v1, v2, v3;
        for (int i = 0; i < _lowerRing.Count - 1; i++)
        {
            v0 = _lowerRing[i];
            v1 = _upperRing[i];
            v2 = _lowerRing[i + 1];
            v3 = _upperRing[i + 1];
            draft.Add(Triangle(v0, v1, v2));
            draft.Add(Triangle(v2, v1, v3));
        }

        v0 = _lowerRing[_lowerRing.Count - 1];
        v1 = _upperRing[_upperRing.Count - 1];
        v2 = _lowerRing[0];
        v3 = _upperRing[0];
        draft.Add(Triangle(v0, v1, v2));
        draft.Add(Triangle(v2, v1, v3));

        return draft;
    }

    public static MeshTemplate Pyramid(float _radius, int _segments, float _heignt, bool _inverted = false)
    {
        MeshTemplate draft = BaselessPyramid(_radius, _segments, _heignt, _inverted);
        List<Vector3> vertices = new List<Vector3>(_segments);
        for (int i = draft.m_Vertices.Count - 2; i >= 0; i -= 3)
        {
            vertices.Add(draft.m_Vertices[i]);
        }
        draft.Add(TriangleFan(vertices));
        draft.m_Name = "Pyramid";
        return draft;
    }

    public static MeshTemplate Cylinder(Vector3 _baseCenter, float _radius, float _height, int _segments, bool _inverted = false)
    {
        {
            float segmentAngle = 360f / _segments;
            float currentAngle = 0;

            List<Vector3> lowerRing = new List<Vector3>(_segments);
            List<Vector3> upperRing = new List<Vector3>(_segments);
            for (int i = 0; i < _segments; i++)
            {
                Vector3 point = Utility.PointOnCircle3YZ(_radius, currentAngle);
                lowerRing.Add(point + Vector3.right * _height / 2 + _baseCenter);
                upperRing.Add(point - Vector3.right * _height / 2 + _baseCenter);
                currentAngle -= segmentAngle;
            }

            MeshTemplate draft = TriangleFan(lowerRing);
            draft.Add(Band(lowerRing, upperRing));
            upperRing.Reverse();
            draft.Add(TriangleFan(upperRing));
            draft.m_Name = "Cylinder";
            return draft;
        }
    } 
}



