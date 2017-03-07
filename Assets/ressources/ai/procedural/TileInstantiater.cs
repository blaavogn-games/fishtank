using UnityEngine;
using System.Collections;

public class TileInstantiater : MonoBehaviour {
    public GameObject Tile;

    void Start () {
        TileHandler[,] th = new TileHandler[16,10];
        for(int x = 0; x < 16; x++) {
            for(int y = 0; y < 10; y++) {
                GameObject g = (GameObject) Instantiate(Tile,
                                                 new Vector3(x * Tile.transform.localScale.x, -20, y * Tile.transform.localScale.z),
                                                 Quaternion.identity);
                th[x, y] = g.GetComponent<TileHandler>();
                if(x - 1 >= 0) {
                    th[x - 1, y].Neighbours[TileHandler.EAST] = th[x, y];
                    th[x, y].Neighbours[TileHandler.WEST] = th[x - 1, y];
                }
                if(y - 1 >= 0){
                    th[x, y - 1].Neighbours[TileHandler.EAST] = th[x, y];
                    th[x, y].Neighbours[TileHandler.WEST] = th[x, y - 1];
                }
             }
        }
    }
}
