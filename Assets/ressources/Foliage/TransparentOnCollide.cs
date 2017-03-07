using UnityEngine;
using System.Collections;

public class TransparentOnCollide : MonoBehaviour {
    public Color AlbedoNormal, AlbedoTrans;
    private Material mat;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
            mat.SetColor("_Albedo", AlbedoTrans);
    }
}
