using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph{
    public readonly Dictionary<Vector3, Vertex> Vertices;
    private int graphSearch = 0;
    private Vertex[] openList = new Vertex[10000]; //Should theoretically be a PQ but this might be sufficient

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

    public bool ColorPath(Vector3 start, Vector3 end, out Stack<Vector3> path) //Dijkstra
    {
        path = new Stack<Vector3>(); //Possibly should be null when return false;
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
            path.Push(cur.Pos);
            //cur.Rep.GetComponent<Renderer>().material.color = Color.green;
            cur = cur.Predecessor;
        }
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
