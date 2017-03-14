using UnityEngine;
using System.Collections;

public class Checkpoint : Trigger {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

    }
    public override void OnEnter(Collider other)
    {
        var playerCol = other.GetComponentInParent<Player>();
        if (playerCol != null)
        {
            if (playerCol.spawnPoint != transform.position)
            {
                playerCol.spawnPoint = transform.position;
                Debug.Log("Spawn set to: "+ playerCol.spawnPoint.ToString());
            }
        }
    }
}
