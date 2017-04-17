using UnityEngine;
using System.Collections;

public class Morane : MonoBehaviour
{
    private enum State {ATTACK};
    private State state = State.ATTACK;
    private PlayerFollowers playerFollowers;
    private Vector3 target;

    private float Speed = 10.0f;

    void Start ()
    {
        playerFollowers = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFollowers>();
    }
	
	void Update ()
    {
        switch (state)
        {
            case State.ATTACK:
                target = playerFollowers.GetTarget().position;
                Vector3 newPosition = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
                Vector3 movement = newPosition - transform.position;
                //wiggle.wiggleSpeed = movement.magnitude * 50 + 10;
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, movement, 0.1f, 0));
                transform.position += transform.forward * movement.magnitude;
                break;
        }


	}
}
