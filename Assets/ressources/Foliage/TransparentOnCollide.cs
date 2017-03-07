using UnityEngine;
using System.Collections;

public class TransparentOnCollide : MonoBehaviour {
    public Material MatNormal, MatTransparent;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.root.tag == "Player")
            rend.material = MatTransparent;
    }

    void OnTriggerExit(Collider col)
    {
        if(col.transform.root.tag == "Player")
            rend.material = MatNormal;
    }
}
