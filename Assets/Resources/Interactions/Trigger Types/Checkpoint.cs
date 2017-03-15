using UnityEngine;
using System.Collections;

public class Checkpoint : Trigger {
    public void OnEnter(Collider other)
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
