using UnityEngine;
using System.Collections;

public class RollingStone : MonoBehaviour
{
    public GameObject FishNet;
    public Rigidbody FinalWeed;
    public HingeJoint[] joints;

    public DragArea FilterDragArea;

    private bool dragged = false;

    private float timer = 0;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!dragged) return;
	    FilterDragArea.DragMultiplier = Mathf.Lerp(1, 15, timer);
	    timer += Time.deltaTime/3;
	    if (timer > 1)
	        timer = 1;
	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject != FishNet)
            return;
        FinalWeed.isKinematic = false;
        foreach (HingeJoint j in joints)
        {
            j.breakForce = 0;
        }
        dragged = true;
    }

}
