using UnityEngine;
using System.Collections.Generic;
using System;

public class Vertex : IComparable{
    public Vector3 Pos;
    public readonly HashSet<Vertex> Neighbors;
    public GameObject Rep; //Temp for debugging
    public int searchInit = 0, searchEnd = 0;
    public float SearchVal = 0, Heur = 0;
    public Vertex Predecessor;

    public Vertex(Vector3 pos, GameObject rep)
    {
        this.Pos = pos;
        this.Rep = rep;
        Neighbors = new HashSet<Vertex>();
    }

    public void AddNeighbor(Vertex v)
    {
        Neighbors.Add(v);
    }

    //Equality and HashCode might not be needed anyway
    public override bool Equals(object obj)
    {
       if (!(obj is Vertex))
          return false;
        Vertex v = (Vertex) obj;
        return this.Pos == v.Pos;
    }

    public override int GetHashCode()
    {
        return Pos.GetHashCode();
    }

    public int CompareTo(object obj)
    {
        if(!(obj is Vertex))
            throw new Exception("Invalid compare");
        Vertex o = (Vertex) obj;
        float tv = SearchVal + Heur;
        float ov = o.SearchVal + Heur;
        if(tv > ov) return 1;
        if(tv < ov) return -1;
        return 0;
    }
}
