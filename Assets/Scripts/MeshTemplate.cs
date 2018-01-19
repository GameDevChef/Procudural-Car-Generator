using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MeshTemplate
{

    public string m_Name;
    public List<Vector3> m_Vertices = new List<Vector3>();
    public List<int> m_Triangles = new List<int>();
    public List<Vector3> m_Normals = new List<Vector3>();
    public List<Vector2> m_UVs = new List<Vector2>();
    public List<Color> m_Colors = new List<Color>();

    public MeshTemplate()
    {
    }

    public MeshTemplate(Mesh _mesh)
    {
        m_Name = _mesh.name;
        m_Vertices.AddRange(_mesh.vertices);
        m_Triangles.AddRange(_mesh.triangles);
        m_Normals.AddRange(_mesh.normals);
        m_UVs.AddRange(_mesh.uv);
        m_Colors.AddRange(_mesh.colors);
    }

    public void Add(MeshTemplate template)
    {        
        for (int i = 0; i < template.m_Triangles.Count; i++)
        {
            m_Triangles.Add(template.m_Triangles[i] + m_Vertices.Count);
        }

        m_Vertices.AddRange(template.m_Vertices);
        m_Normals.AddRange(template.m_Normals);
        m_UVs.AddRange(template.m_UVs);
        m_Colors.AddRange(template.m_Colors);
    }

    public Mesh ToMesh()
    {
        Mesh mesh = new Mesh { name = m_Name };
        mesh.SetVertices(m_Vertices);
        mesh.SetTriangles(m_Triangles, 0);
        mesh.SetNormals(m_Normals);
        mesh.SetUVs(0, m_UVs);
        mesh.SetColors(m_Colors);
        mesh.RecalculateNormals();
        return mesh;
    }

    public void FlipTriangles()
    {
        int[] triangles = m_Triangles.ToArray();
        for (int j = 0; j < triangles.Length; j += 3)
        {
             Utility.Swap(ref triangles[j], ref triangles[j + 1]);
        }
        m_Triangles = triangles.ToList();
    }

    public void FlipNormals()
    {
        List<Vector3> normals = m_Normals;
        for (int i = 0; i < normals.Count; i++)
        {
            normals[i] = -normals[i];
        }
        m_Normals = normals;
    }

    public void FlipFaces()
    {
        FlipTriangles();
        FlipNormals();
    }
}