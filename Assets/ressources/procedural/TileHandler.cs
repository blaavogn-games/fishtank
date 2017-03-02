using UnityEngine;
using System.Collections;

public class TileHandler : MonoBehaviour
{
    public const int NORTH = 0, WEST = 1, SOUTH = 2, EAST = 3;
    public TileHandler[] Neighbours = new TileHandler[4];
    public int Height;
    public bool Visible = false;

    void OnBecameVisible()
    {
        Visible = true;
        float refHeight = transform.position.y;
        int c = 1;
        foreach(TileHandler t in Neighbours)
        {
            if(t != null && t.Visible) {
                refHeight += t.transform.position.y;
                c++;
            }
        }
        transform.position = new Vector3(transform.position.x,
                                         refHeight/c + (Random.Range(-2,3) * 4),
                                         transform.position.z);
        GetComponent<Renderer>().material.color =
            new Color(Random.Range(0,100) / 100.0f,
                      Random.Range(0,100) / 100.0f,
                      Random.Range(0,100) / 100.0f);
    }

    void OnBecameInvisible()
    {
        Visible = false;
    }
}
