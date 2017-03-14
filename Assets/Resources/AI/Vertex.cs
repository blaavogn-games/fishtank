using UnityEngine;
using System.Collections.Generic;

public class Vertex{
    public Vector3 Pos;
    public readonly HashSet<Vertex> Neighbors;
    public GameObject Rep; //Temp for debugging
    public int searchInit = 0, searchEnd = 0;
    public float SearchVal = 0;
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
}
