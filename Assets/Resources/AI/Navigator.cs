using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour {
    public GameObject Vert; //preFab for debugmode
    public bool DebugMode = false;
    private const float SPACING = 10;
    public GameObject Bound1, Bound2;
    private Vector3 minLimit = Vector3.zero, maxLimit = Vector3.zero;
    private int xSize, ySize, zSize;
    private Graph graph;
    private System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

    void Awake () {
        Scene scene = SceneManager.GetActiveScene();
        if(File.Exists("data/"+scene.name)) {
            Deserialize("data/"+scene.name);
            return;
        }

        Vector3 b1 = Bound1.transform.position;
        Vector3 b2 = Bound2.transform.position;
        minLimit.x = ((int) ((Mathf.Min(b1.x, b2.x) + 5) / 10.0f)) * 10;
        minLimit.y = ((int) ((Mathf.Min(b1.y, b2.y) + 5) / 10.0f)) * 10;
        minLimit.z = ((int) ((Mathf.Min(b1.z, b2.z) + 5) / 10.0f)) * 10;
        maxLimit.x = ((int) ((Mathf.Max(b1.x, b2.x) + 5) / 10.0f)) * 10;
        maxLimit.y = ((int) ((Mathf.Max(b1.y, b2.y) + 5) / 10.0f)) * 10;
        maxLimit.z = ((int) ((Mathf.Max(b1.z, b2.z) + 5) / 10.0f)) * 10;
        stopWatch.Start();
        float DSPACING = Mathf.Sqrt(SPACING * SPACING + SPACING * SPACING);
        float DDSPACING = Mathf.Sqrt(DSPACING * DSPACING + SPACING * SPACING);

        //Building graph
        graph = new Graph();
        Vector3 _vertSize = (maxLimit - minLimit) / SPACING;
        xSize = (int) Mathf.Ceil(_vertSize.x) + 1;
        ySize = (int) Mathf.Ceil(_vertSize.y) + 1;
        zSize = (int) Mathf.Ceil(_vertSize.z) + 1;

        for (int x = 0; x < xSize; x++)
            for(int y = 0; y < ySize; y++)
                for(int z = 0; z < zSize; z++)
                {
                    Vector3 placement = new Vector3(minLimit.x + x * SPACING,
                                                   minLimit.y + y * SPACING,
                                                   minLimit.z + z * SPACING);
                    GameObject g = DebugMode ? (GameObject) Instantiate(Vert, placement, Quaternion.identity) : null;
                    graph.AddVertex(new Vertex(placement, g));
                }

        foreach (KeyValuePair<Vector3, Vertex> kvp in graph.Vertices)
        {
            TryCreateEdge(kvp.Key, Vector3.up, SPACING, Color.red);
            TryCreateEdge(kvp.Key, Vector3.forward, SPACING, Color.green);
            TryCreateEdge(kvp.Key, Vector3.left, SPACING, Color.blue);
            TryCreateEdge(kvp.Key, Vector3.up + Vector3.forward, DSPACING, Color.cyan);
            TryCreateEdge(kvp.Key, Vector3.up + Vector3.forward * -1, DSPACING, Color.cyan);
            TryCreateEdge(kvp.Key, Vector3.up + Vector3.left, DSPACING, Color.magenta);
            TryCreateEdge(kvp.Key, Vector3.up + Vector3.left * -1, DSPACING, Color.magenta);
            TryCreateEdge(kvp.Key, Vector3.forward + Vector3.left, DSPACING, Color.yellow);
            TryCreateEdge(kvp.Key, Vector3.forward + Vector3.left * -1, DSPACING, Color.yellow);
            TryCreateEdge(kvp.Key, Vector3.up + Vector3.forward + Vector3.left, DDSPACING, Color.blue);
            TryCreateEdge(kvp.Key, Vector3.up + Vector3.forward + Vector3.left * -1, DDSPACING, Color.blue);
            TryCreateEdge(kvp.Key, Vector3.up + Vector3.forward * -1 + Vector3.left, DDSPACING, Color.blue);
            TryCreateEdge(kvp.Key, Vector3.up + Vector3.forward * -1 + Vector3.left * -1, DDSPACING, Color.blue);
        }
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        Debug.Log(String.Format("Navigation graph build in {0}ms, DebugMode: {1}", ts.Milliseconds, DebugMode));
        Serialize();
    }

    private void Deserialize(string path)
    {
        int intSize = 4;
        int floatSize = 4;
        using (var stream = File.Open(path, FileMode.Open))
        {
            Dictionary<Vector3,Vertex> dic = new Dictionary<Vector3, Vertex>();
            byte[] buf = new byte[16];
            stream.Read(buf, 0, intSize);
            int dicSize = BitConverter.ToInt32(buf, 0);
            Vector3[] v3 = new Vector3[dicSize];
            Vertex[] ve = new Vertex[dicSize];
            Vector3[][] neigh = new Vector3[dicSize][];
            for (int i = 0; i < dicSize; i++)
            {
                stream.Read(buf, 0, floatSize);
                float x = BitConverter.ToSingle(buf, 0);
                stream.Read(buf, 0, floatSize);
                float y = BitConverter.ToSingle(buf, 0);
                stream.Read(buf, 0, floatSize);
                float z = BitConverter.ToSingle(buf, 0);
                v3[i] = new Vector3(x,y,z);
                ve[i] = new Vertex(v3[i], null);
                minLimit.x = Mathf.Min(minLimit.x, x);
                maxLimit.x = Mathf.Max(maxLimit.x, x);
                minLimit.y = Mathf.Min(minLimit.y, y);
                maxLimit.y = Mathf.Max(maxLimit.y, y);
                minLimit.z = Mathf.Min(minLimit.z, z);
                maxLimit.z = Mathf.Max(maxLimit.z, z);
                stream.Read(buf, 0, intSize);
                int neighSize = BitConverter.ToInt32(buf, 0);
                neigh[i] = new Vector3[neighSize];
                dic.Add(v3[i], ve[i]);
                for (int j = 0; j < neighSize; j++)
                {
                    stream.Read(buf, 0, floatSize);
                    float vx = BitConverter.ToSingle(buf, 0);
                    stream.Read(buf, 0, floatSize);
                    float vy = BitConverter.ToSingle(buf, 0);
                    stream.Read(buf, 0, floatSize);
                    float vz = BitConverter.ToSingle(buf, 0);
                    neigh[i][j] = new Vector3(vx,vy,vz);
                }
            }

            for(int i = 0; i < dicSize; i++)
            {
                Vertex v = ve[i];
                for(int j = 0; j < neigh[i].Length; j++)
                {
                    v.AddNeighbor(dic[neigh[i][j]]);
                }
            }
            graph = new Graph(dic);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Debug.Log(String.Format("Deserialize graph {0}ms", ts.Milliseconds, DebugMode));
            stream.Close();
        }
    }

    private void Serialize()
    {
        Scene scene = SceneManager.GetActiveScene();
        using (var stream = File.Open("data/"+scene.name, FileMode.OpenOrCreate))
        {
            //Layout
            //1. Dic size
            //2. kvp
            //2a. Pos
            //2b. Vert
            //2ba. Neghbors
            Write(stream, BitConverter.GetBytes(graph.Vertices.Count));
            foreach(var kvp in graph.Vertices)
            {
                Vector3 vec = kvp.Key;
                Vertex verts = kvp.Value;
                Write(stream, BitConverter.GetBytes(vec.x));
                Write(stream, BitConverter.GetBytes(vec.y));
                Write(stream, BitConverter.GetBytes(vec.z));
                Write(stream, BitConverter.GetBytes(verts.Neighbors.Count));
                foreach(Vertex v3 in verts.Neighbors) {
                    Write(stream, BitConverter.GetBytes(v3.Pos.x));
                    Write(stream, BitConverter.GetBytes(v3.Pos.y));
                    Write(stream, BitConverter.GetBytes(v3.Pos.z));
                }
            }
            stream.Close();
        }
    }

    private void Write(FileStream stream, byte[] buffer)
    {
        stream.Write(buffer, 0, buffer.Length);
    }

    public bool TryFindPath(Vector3 start, Vector3 end, out List<Vector3> path)
    {
        if(InSight(start, end-start, (end-start).magnitude, Color.blue))
        {
            path = new List<Vector3>();
            path.Add(end);
            return true;
        }
        bool ret = graph.ColorPath(graph.FindClosest(start), graph.FindClosest(end), out path);
        return ret;
    }

    private Vector3 Snap(Vector3 v)
    {
        //Snap should take scale into account so a valid vector is always found.
        return new Vector3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
    }

    private void TryCreateEdge(Vector3 pos, Vector3 dir, float length, Color col)
    {
        Vector3 target = Snap(pos + dir.normalized * length);
        if(InSight(pos, target - pos, length, col))
            graph.AddEdge(pos, target);
    }

    private bool InSight(Vector3 pos, Vector3 dir, float length, Color col)
    {
        Vector3 target = pos + dir;
        if(target.x > maxLimit.x || target.y > maxLimit.y || target.z > maxLimit.z ||
           target.x < minLimit.x || target.y < minLimit.y || target.z < minLimit.z)
            return false;
        Ray ray = new Ray(pos, dir);
        Ray ray2 = new Ray(target, dir * -1);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * length, col, 10.5f);
        return (!Physics.SphereCast(ray.origin, 0.01f, ray.direction, out hit, length, 1) &&
            !Physics.SphereCast(ray2.origin, 0.01f, ray2.direction, out hit, length, 1)); //Possible figure out collision layers
    }
}
