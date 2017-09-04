using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour {

    public int width = 10;
    public int height = 10;
    public float scale = 20f;
    public int seed = 41145;

    public Material matTest;

    void Start () {

        GameObject c = new GameObject(name = "TerrainContainer");
        
        for (int i = 0; i < width; i++)
        {
            for(int t = 0; t < height; t ++)
            {
                float[] heights = new float[4];

                heights[0] = CalcHeight(i, t) * scale;
                heights[1] = CalcHeight(i + 1, t) * scale;
                heights[2] = CalcHeight(i , t + 1) * scale;
                heights[3] = CalcHeight(i + 1, t + 1) * scale;

                Vector3[] points = new Vector3[4];
                points[0] = new Vector3(i, heights[0], t) * scale;
                points[1] = new Vector3(i + 1, heights[1], t) * scale;
                points[2] = new Vector3(i, heights[2], t + 1) * scale;
                points[3] = new Vector3(i + 1, heights[3], t + 1) * scale; 

                GameObject g = new GameObject(name = i.ToString() + " " + t.ToString());

                g.transform.parent = c.transform;
                MeshFilter mf = g.AddComponent<MeshFilter>();
                MeshRenderer mr = g.AddComponent<MeshRenderer>() as MeshRenderer;
                mr.material = matTest;

                Rigidbody r = g.AddComponent<Rigidbody>() as Rigidbody;
                r.constraints = RigidbodyConstraints.FreezeAll;
                r.isKinematic = true;

                MeshCollider mc = g.AddComponent<MeshCollider>();
                AddMesh(g, points);

                mc.sharedMesh = mf.mesh;
            }
        }

    }

    float CalcHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    void AddMesh(GameObject obj, Vector3[] points)
    {
        MeshFilter mf = obj.GetComponent<MeshFilter>();
        Mesh m = new Mesh();
        mf.mesh = m;
        m.vertices = points;

        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;
        m.triangles = triangles;

        Vector3[] normals = new Vector3[4];
        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;
        m.normals = normals;

        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);
        m.uv = uv;

        m.RecalculateNormals();
        m.RecalculateTangents();
        m.RecalculateBounds();


    }

}

