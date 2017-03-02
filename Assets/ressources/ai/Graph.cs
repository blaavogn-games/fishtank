using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph{
    public readonly Dictionary<Vector3, Vertex> Vertices;
    private int graphSearch = 0;
    //Possibly autoresize or List<Vertex>
    private Vertex[] openList = new Vertex[3000]; //Should theoretically be a PQ but this might be sufficient

    public Graph()
    {
        Vertices = new Dictionary<Vector3, Vertex>();
    }

    public void AddVertex(Vertex v)
    {
        Vertices.Add(v.Pos, v);
    }

    public void AddEdge(Vector3 v1, Vector3 v2)
    {
        Vertex ve1 = Vertices[v1];
        Vertex ve2 = Vertices[v2];
        ve1.AddNeighbor(ve2);
        ve2.AddNeighbor(ve1);
    }

    //Runs O(N) probably not important could implement a KD-Tree
    //or if Vertices are regularly spread make a snap function
    public Vector3 FindClosest(Vector3 v)
    {
        float minDist = float.MaxValue;
        Vector3 closest = v;
        foreach(KeyValuePair<Vector3, Vertex> kvp in Vertices)
        {
            float dist = (kvp.Key - v).magnitude;
            if(minDist > dist)
            {
                minDist = dist;
                closest = kvp.Key;
            }
        }
        return closest;
    }

    public bool ColorPath(Vector3 start, Vector3 end, out List<Vector3> path) //Dijkstra
    {
        path = new List<Vector3>(); //Possibly should be null when return false;
        Vertex begin = Vertices[start];
        Vertex target = Vertices[end];
        begin.Predecessor = null;
        int head = 0;
        graphSearch++;
        openList[head++] = begin;
        bool pathFound = false;
        while(head > 0)
        {
            Vertex v = openList[--head];
            if(v == target){
                pathFound = true;
                break;
            }
            v.searchEnd = graphSearch;
            foreach(Vertex n in v.Neighbors)
            {
                if(n.searchEnd == graphSearch)
                    continue;
                if(n.searchInit < graphSearch)
                {
                    n.SearchVal = float.PositiveInfinity;
                    n.searchInit = graphSearch;
                }

                float pathVal = v.SearchVal + (v.Pos - n.Pos).magnitude;
                if (pathVal < n.SearchVal)
                {
                    n.SearchVal = pathVal;
                    n.Predecessor = v;
                    openList[head++] = n;
                    int swap = head - 1;
                    while(swap > 0 && openList[swap].SearchVal > openList[swap - 1].SearchVal)
                    {
                        Vertex tmp = openList[swap];
                        openList[swap] = openList[swap - 1];
                        openList[swap - 1] = tmp;
                        swap--;
                    }
                }
            }
        }
        if(!pathFound) {
            return false;
        }
        //Color - Collecting path
        Vertex cur = target;
        while(cur != null)
        {
            path.Add(cur.Pos);
            //cur.Rep.GetComponent<Renderer>().material.color = Color.green;
            cur = cur.Predecessor;
        }
        path.Reverse();
        return true;
    }

    public void ColorConnected(Vector3 v1)
    {
        graphSearch++;
        Stack<Vertex> openList = new Stack<Vertex>();
        openList.Push(Vertices[v1]);
        while (openList.Count > 0)
        {
            Vertex v = openList.Pop();
            v.Rep.GetComponent<Renderer>().material.color = Color.red;
            foreach(Vertex n in v.Neighbors)
            {
                if(n.searchInit < graphSearch) {
                    n.searchInit = graphSearch;
                    openList.Push(n);
                }
            }
        }
    }
}
