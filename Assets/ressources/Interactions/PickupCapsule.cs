using UnityEngine;
using System.Collections;

public class PickupCapsule : MonoBehaviour {
    public Transform Player;

    void Update () {
        transform.Rotate(30 * Time.deltaTime, 20 * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var script = collision.gameObject.GetComponent<Player>();
        if (script != null)
            DestroyObject(this.gameObject);
    }
}
