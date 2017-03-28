using System.Collections.Generic;using UnityEngine;

public class Graph{
    public readonly Dictionary<Vector3, Vertex> Vertices;
    private int graphSearch = 0;
    //Possibly autoresize or List<Vertex>

    public Graph()
    {
        Vertices = new Dictionary<Vector3, Vertex>();
    }

    public Graph(Dictionary<Vector3, Vertex> vertices)
    {
        this.Vertices = vertices;
    }

    public void AddVertex(Vertex v)
    {
        if(v.Pos.x == 10.0f && v.Pos.y == 0.0f)
            Debug.Log(v.Pos);
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
        v.x =  ((int)( (v.x + 5) / 10.0f)) * 10;
        v.y =  ((int)( (v.y + 5) / 10.0f)) * 10;
        v.z =  ((int)( (v.z + 5) / 10.0f)) * 10;
        return Vertices[v].Pos;
    }

    public bool ColorPath(Vector3 start, Vector3 end, out List<Vector3> path) //Dijkstra
    {
        C5.IntervalHeap<Vertex> openList = new C5.IntervalHeap<Vertex>();
        path = new List<Vector3>(); //Possibly should be null when return false;
        Vertex begin = Vertices[start];
        Vertex target = Vertices[end];
        begin.Predecessor = null;
        graphSearch++;
        openList.Add(begin);
        bool pathFound = false;
        int count = 0;
        while(!openList.IsEmpty)
        {
            Vertex v = openList.DeleteMin();
            count++;
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
                    n.Heur = Vector3.Distance(n.Pos, end);
                    n.searchInit = graphSearch;
                }
                float pathVal = v.SearchVal + Vector3.Distance(v.Pos, n.Pos);
                if (pathVal < n.SearchVal)
                {
                    n.SearchVal = pathVal;
                    n.Predecessor = v;
                    openList.Add(n);
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
        openList.Push(Vertices[FindClosest(v1)]);
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
