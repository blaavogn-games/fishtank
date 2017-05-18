using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour
{
    public enum TriggerType { Checkpoint};
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

/*    [HideInInspector] public float hunger = 0;
    [HideInInspector] public bool relative = false;
    [HideInInspector] public GameObject triggerObject;
    [HideInInspector] public GameObject scriptObject;
    [HideInInspector] public bool activateGameObject = false;*/

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
        if (other.root.gameObject == player && !entered && playerScript != null && Type==TriggerType.Checkpoint)
        {
            World.i.CheckPoint(gameObject);
            if (Once)
                entered = true;
        }
       /* else if (other.root.gameObject == triggerObject && !entered && Type == TriggerType.TriggerByOther)
        {
            if (scriptObject == null)
                Debug.LogError("No object to instantiate from trigger");
            else
            {
                if(!activateGameObject)
                    Instantiate(scriptObject);
                else
                    scriptObject.SetActive(true);
            }
            if (Once)
                entered = true;
        }*/
    }
}
