using UnityEngine;
using System.Collections;

public class ObjectTrigger : MonoBehaviour
{
    public GameObject CollisionObject;
    public GameObject TriggerScript;
    public bool InstantiateObject = false;
    public bool Once = true;
    private bool entered = false;

    // Use this for initialization
    void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        CollideHandler(other.gameObject);
    }

    // Update is called once per frame
    void Update () {
	
	}
    private void CollideHandler(GameObject other)
    {
        if (other == CollisionObject && !entered)
         {
             if (TriggerScript == null)
                 Debug.LogError("No object to instantiate from trigger");
             else
             {
                 if(InstantiateObject)
                     Instantiate(TriggerScript);
                 else
                     TriggerScript.SetActive(true);
             }
             if (Once)
                 entered = true;
         }
    }
}
