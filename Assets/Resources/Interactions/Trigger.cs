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
//    private bool exited = false;

    [HideInInspector]
    public float hunger = 0;
    [HideInInspector]
    public bool relative = false;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
	}
    public void TriggerByParent(Collision other)
    {
        CollideHandler(other.transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        CollideHandler(other.transform);
    }

    private void CollideHandler(Transform other)
    {
        if (other.root.gameObject == player && !entered && playerScript != null)
        {
            switch (Type)
            {
                case TriggerType.Checkpoint:
                    World.i.CheckPoint(gameObject);
                    Debug.Log("Checkpoint at " + transform.position);
                    break;
            }
            if (Once)
                entered = true;
        }
    }
}
