using UnityEngine;
using System.Collections;

public class DisableRendererOnRuntime : MonoBehaviour {
    void Start () {
        GetComponent<Renderer>().enabled = false;
    }
}
