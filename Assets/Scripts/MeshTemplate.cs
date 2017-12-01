using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MeshTemplate
{

    public string name = "";
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> uv = new List<Vector2>();
    public List<Color> colors = new List<Color>();

    public MeshTemplate()
    {
    }

    public MeshTemplate(Mesh mesh)
    {
        name = mesh.name;
        vertices.AddRange(mesh.vertices);
        triangles.AddRange(mesh.triangles);
        normals.AddRange(mesh.normals);
        uv.AddRange(mesh.uv);
        colors.AddRange(mesh.colors);
    }

    public void Add(MeshTemplate template)
    {
        foreach (var triangle in template.triangles)
        {
            triangles.Add(triangle + vertices.Count);
        }
        vertices.AddRange(template.vertices);
        normals.AddRange(template.normals);
        uv.AddRange(template.uv);
        colors.AddRange(template.colors);
    }

    public Mesh ToMesh()
    {
        Mesh mesh = new Mesh { name = name };
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uv);
        mesh.SetColors(colors);
        mesh.RecalculateNormals();
        return mesh;
    }

    public void FlipTriangles()
    {

        int[] triangles = this.triangles.ToArray();
        for (int j = 0; j < triangles.Length; j += 3)
        {
             Utility.Swap(ref triangles[j], ref triangles[j + 1]);
        }
        this.triangles = triangles.ToList();


    }

    public void FlipNormals()
    {
        var normals = this.normals;
        for (int i = 0; i < normals.Count; i++)
        {
            normals[i] = -normals[i];
        }
        this.normals = normals;
    }

    public void FlipFaces()
    {
        FlipTriangles();
        FlipNormals();
    }
}