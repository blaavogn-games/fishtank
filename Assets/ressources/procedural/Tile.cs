using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    void Start () {
    }

    void OnBecameVisible()
    {
        GetComponent<Renderer>().material.color = new Color(Random.Range(0,100) / 100.0f,Random.Range(0,100) / 100.0f,Random.Range(0,100) / 100.0f);
    }
}
