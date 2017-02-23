using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Navigator : MonoBehaviour {
    public GameObject Vert; //preFab for debugmode
    public bool DebugMode = false;
    private const float SPACING = 1;
    private Vector3
        minLimit = new Vector3(0, 0, 0),
        maxLimit = new Vector3(20, 20, 20);
    private int xSize, ySize, zSize;
    private Graph graph;
    private System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

    void Awake () {
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
    //graph.ColorConnected(Vector3.zero);
        //Stack<Vector3> p;
        //var b = graph.ColorPath(Vector3.zero, new Vector3(4,4,4), out p);
    }

    public bool TryFindPath(Vector3 start, Vector3 end, out Stack<Vector3> path)
    {
        stopWatch.Reset();
        bool ret = graph.ColorPath(Snap(start), Snap(end), out path);
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        Debug.Log(String.Format("Path found in {0}ms, DebugMode: {1}", ts.Milliseconds, DebugMode));
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
        if(target.x > maxLimit.x || target.y > maxLimit.y || target.z > maxLimit.z ||
           target.x < minLimit.x || target.y < minLimit.y || target.z < minLimit.z)
            return;
        Ray ray = new Ray(pos, dir);
        Ray ray2 = new Ray(target, dir * -1);
        RaycastHit hit;
        if (!Physics.Raycast(ray.origin, ray.direction, out hit, length, 1) &&
            !Physics.Raycast(ray2.origin, ray2.direction, out hit, length, 1)) //Possible figure out collision layers
            graph.AddEdge(pos, target);
    }
}
