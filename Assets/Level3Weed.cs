using UnityEngine;
using System.Collections;

public class Level3Weed : MonoBehaviour
{
    public enum State
    {
        IDLE,
        ONPLAYER,
        ONMORAY
    };

    public State state = State.IDLE;

    private HingeJoint joint;
    private Rigidbody body;
    private Rigidbody playerBody;
    private Player player;
    public Level3End Level3Script;
    public PostLevelScript PostLevel;

	// Use this for initialization
	void Start ()
	{
	    joint = GetComponentInParent<HingeJoint>();
	    body = GetComponentInParent<Rigidbody>();
	    playerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
	    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other){
        if (state == State.IDLE && other.attachedRigidbody == playerBody)
        {
            state = State.ONPLAYER;
            joint.connectedBody = playerBody;
            body.isKinematic = false;
        }
    }
}
