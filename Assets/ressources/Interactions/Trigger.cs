using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour
{
    public enum TriggerType { Checkpoint, SetHunger };
    public TriggerType Type = TriggerType.Checkpoint;
    //Whether checkpoint can be reused
    [Tooltip("Whether this checkpoint can be used multiple times")]
    public bool Once = false;

    //This is the player
    private GameObject player;
    private Player playerScript;
    //private Collider other;
    private bool entered = false;
    private bool exited = false;

    [HideInInspector]
    public float hunger = 0;
    [HideInInspector]
    public bool relative = false;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject == player && !entered && playerScript!=null)
        {
            switch (Type)
            {
                case TriggerType.Checkpoint:
                    CheckPointEnter();
                    break;
                case TriggerType.SetHunger:
                    SetHungerEnter();
                    break;
            }
            if(Once)
                entered = true;
        }
    }
    private void CheckPointEnter()
    {
        if (playerScript.spawnPoint != transform.position)
        {
            playerScript.spawnPoint = transform.position;
            Debug.Log("Spawn set to: " + playerScript.spawnPoint.ToString());
        }
    }
    private void SetHungerEnter()
    {
        if (relative)
            playerScript.hunger += hunger;
        else
            playerScript.hunger = hunger;
    }
}
