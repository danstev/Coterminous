using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour {

    public int width = 10;
    public int height = 10;

    float lowest = 1f;
    float highest = 1f;

    public int genAmount = 5;

    public float scale = 20f;
    public float XYscale = 1f;
    public int seed = 41145;

    public Material matTest;
    public Gradient gradient;

    void Start () {

        int currentX = (int)transform.position.x - ((genAmount / 2) * 10);
        int currentY = (int)transform.position.y - ((genAmount / 2) * 10);

        for(int i = 0; i < genAmount; i++)
        {
            for(int y = 0; y < genAmount; y++)
            {
                GenTerrainOnSpot(currentX + (i * 10), currentY + (y * 10));
            }
        }

        UnloadUnneeded(currentX, currentY);


    }

    void GenTerrainOnSpot(int x, int y)
    {
        GameObject c = new GameObject(name = x + " " + y);
        c.tag = "Terrain";
        

        for (int i = 0; i < 10; i++)
        {
            for (int t = 0; t < 10; t++)
            {
                float[] heights = new float[4];
                heights[0] = CalcHeight(i + seed + x, t + seed + y) * scale;
                heights[1] = CalcHeight(i + seed + 1 + x, t + seed + y) * scale;
                heights[2] = CalcHeight(i + seed + x, t + 1 + seed + y) * scale;
                heights[3] = CalcHeight(i + 1 + seed + x, t + 1 + seed + y) * scale;

                Vector3[] points = new Vector3[4];
                points[0] = new Vector3(i + x, heights[0], t + y) * XYscale;
                points[1] = new Vector3(i + 1 + x, heights[1], t + y) * XYscale;
                points[2] = new Vector3(i + x, heights[2], t + 1 + y) * XYscale;
                points[3] = new Vector3(i + 1 + x, heights[3], t + 1 + y) * XYscale;

                GameObject g = new GameObject(x + i.ToString() + " " + y + t.ToString());
                g.isStatic = true;

                float height = (heights[0] + heights[1] + heights[2] + heights[3]) / 4;

                if (height < lowest)
                {
                    lowest = height;
                }

                if (height > highest)
                {
                    highest = height;
                }

                g.transform.parent = c.transform;
                MeshFilter mf = g.AddComponent<MeshFilter>();
                MeshRenderer mr = g.AddComponent<MeshRenderer>() as MeshRenderer;
                mr.material = matTest;
                float co = Mathf.InverseLerp(lowest, highest, height);
                float coo = Mathf.Round(co * 10f) / 10f;

                mr.material.color = gradient.Evaluate(coo);
                Rigidbody r = g.AddComponent<Rigidbody>() as Rigidbody;
                r.constraints = RigidbodyConstraints.FreezeAll;
                r.isKinematic = true;

                MeshCollider mc = g.AddComponent<MeshCollider>();
                AddMesh(g, points);

                mc.sharedMesh = mf.mesh;
            }
        }

        
        
        
        MeshFilter[] meshFilters = c.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int u = 0;
        
        while (u < meshFilters.Length)
        {
            combine[u].mesh = meshFilters[u].sharedMesh;
            combine[u].transform = meshFilters[u].transform.localToWorldMatrix;
            //meshFilters[u].gameObject.SetActive(false);
            u++;
        }

        
        c.AddComponent<MeshFilter>();
        MeshFilter cmfc = c.GetComponent<MeshFilter>();
        cmfc.mesh = new Mesh();
        cmfc.mesh.CombineMeshes(combine);
        c.gameObject.SetActive(true);
        MeshCollider cMeshCol = c.AddComponent<MeshCollider>();
        cMeshCol.sharedMesh = cmfc.mesh;
        c.AddComponent<GenerateOnCollide>();
        
    }

    IEnumerator GenTerrainOnSpotCo(int x, int y,float lowest, float highest)
    {
        GameObject c = new GameObject(name = x + " " + y);

        for (int i = 0; i < 10; i++)
        {
            for (int t = 0; t < 10; t++)
            {
                float[] heights = new float[4];

                heights[0] = CalcHeight(i + seed + x, t + seed + y) * scale;
                heights[1] = CalcHeight(i + seed + 1 + x, t + seed + y) * scale;
                heights[2] = CalcHeight(i + seed + x, t + 1 + seed + y) * scale;
                heights[3] = CalcHeight(i + 1 + seed + x, t + 1 + seed + y) * scale;

                Vector3[] points = new Vector3[4];
                points[0] = new Vector3(i + x, heights[0], t + y) * scale;
                points[1] = new Vector3(i + 1 + x, heights[1], t + y) * scale;
                points[2] = new Vector3(i + x, heights[2], t + 1 + y) * scale;
                points[3] = new Vector3(i + 1 + x, heights[3], t + 1 + y) * scale;

                GameObject g = new GameObject(x + i.ToString() + " " + y + t.ToString());

                float height = (heights[0] + heights[1] + heights[2] + heights[3]) / 4;

                if (height < lowest)
                {
                    lowest = height;
                }

                if (height > highest)
                {
                    highest = height;
                }

                g.transform.parent = c.transform;
                MeshFilter mf = g.AddComponent<MeshFilter>();
                MeshRenderer mr = g.AddComponent<MeshRenderer>() as MeshRenderer;
                mr.material = matTest;
                mr.material.color = gradient.Evaluate(Mathf.InverseLerp(lowest, highest, height));
                print(Mathf.InverseLerp(lowest, highest, height));
                Rigidbody r = g.AddComponent<Rigidbody>() as Rigidbody;
                r.constraints = RigidbodyConstraints.FreezeAll;
                r.isKinematic = true;

                MeshCollider mc = g.AddComponent<MeshCollider>();
                AddMesh(g, points);

                mc.sharedMesh = mf.mesh;
            }
        }

        yield return new WaitForEndOfFrame();
    }

    float CalcHeight(int x, int y)
    {
        float xCoord = (float)x / height * scale;
        float yCoord = (float)y / width * scale;

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

    void UnloadUnneeded(int x, int y)
    {
        GameObject[] terr = GameObject.FindGameObjectsWithTag("Terrain");
        foreach(GameObject t in terr)
        {
            string str = t.name;
            string[] coords = str.Split(' ');
            int xCoord = int.Parse(coords[0]);
            int yCoord = int.Parse(coords[1]);

            if(xCoord < x || xCoord > x + (genAmount * 10))
            {
                t.SetActive(false);
                print("Unloaded: " + x + " " + y);
            }

            if (yCoord < y || yCoord > y + (genAmount * 10))
            {
                t.SetActive(false);
                print("Unloaded: " + x + " " + y);
            }
        }
    }

    bool CheckIfAlreadyGenerated(int x, int y)
    {
        bool check = false;
        string nameToCheck = x + " " + y;
        GameObject[] terr = GameObject.FindGameObjectsWithTag("Terrain");
        foreach (GameObject t in terr)
        {
            if(t.name == nameToCheck)
            {
                return true;
            }
        }

        return check;
    }




}

