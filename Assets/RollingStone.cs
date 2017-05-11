using UnityEngine;
using System.Collections;

public class RollingStone : MonoBehaviour
{
    public GameObject FishNet;
    public Rigidbody FinalWeed;
    public HingeJoint[] joints;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
    }

}
