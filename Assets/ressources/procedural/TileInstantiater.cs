using UnityEngine;
using System.Collections;

public class TileInstantiater : MonoBehaviour {
    public GameObject[] Tiles;

    void Start () {
        for(int x = 0; x < 16; x++)
            for(int y = 0; y < 10; y++)
                Instantiate(Tiles[0], new Vector3(x * 30, 0, y * 30), Quaternion.identity);
    }

    void Update () {
    }
}
