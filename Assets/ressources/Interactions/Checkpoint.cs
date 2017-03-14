using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
    //Whether checkpoint can be reused
    [Tooltip("Whether this checkpoint can be used multiple times")]
    public bool Once = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

    }
    private void OnTriggerEnter(Collider other)
    {
        var playerCol = other.GetComponentInParent<Player>();
        if (playerCol != null)
        {
            if (playerCol.spawnPoint != transform.position)
            {
                playerCol.spawnPoint = transform.position;
                if (Once) Destroy(this.gameObject);
                Debug.Log(playerCol.spawnPoint);
            }
        }
    }
}
