using UnityEngine;
using System.Collections;

public class PickupCapsule : MonoBehaviour {
    public Transform Player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
    private void OnCollisionEnter(Collision collision)
    {
        var script = collision.gameObject.GetComponent<Player>();
        if (script != null)
            DestroyObject(this.gameObject);
    }
}
