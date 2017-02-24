using UnityEngine;
using System.Collections;

public class TileInstantiater : MonoBehaviour {
    public GameObject Tile;

    void Start () {
        for(int x = 0; x < 16; x++)
            for(int y = 0; y < 10; y++)
                Instantiate(Tile, new Vector3(x * Tile.transform.localScale.x, 0, y * Tile.transform.localScale.z), Quaternion.identity);
    }
}
