using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour
{
    public enum TriggerType { Checkpoint };
    public TriggerType Type = TriggerType.Checkpoint;
    [Tooltip("If this is not set, it will be set to PlayerMesh")]
    public GameObject TriggerObject=null;
    //Whether checkpoint can be reused
    [Tooltip("Whether this checkpoint can be used multiple times")]
    public bool Once = false;
    
    private bool entered = false;
    private bool exited = false;
    Trigger trigger;

    // Use this for initialization
    void Start () {
        if (TriggerObject == null)
        {
            TriggerObject = GameObject.FindGameObjectWithTag("Player");
        }
	    switch (Type)
        {
            case TriggerType.Checkpoint:
                trigger = gameObject.AddComponent<Checkpoint>();
                break;
            default:
                trigger = gameObject.AddComponent<Checkpoint>();
                break;
        }
        trigger.TriggerObject = TriggerObject;
        trigger.Type = Type;
        trigger.Once = Once;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == TriggerObject && !entered)
        {
            OnEnter(other);
            if(Once)
                entered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == TriggerObject && !exited)
        {
            OnExit(other);
            if (Once)
                exited = true;
        }
    }
    public virtual void OnEnter(Collider other){}
    public virtual void OnExit(Collider other){}
}
