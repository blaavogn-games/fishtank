using UnityEngine;
using System.Collections;

public class Morane : MonoBehaviour
{
    private enum State {ATTACK, RETRACT, IDLE};
    private State state = State.ATTACK;
    private PlayerFollowers playerFollowers;
    private Vector3 initialPosition, target;

    private float Speed = 10.0f;
    private float AttackDistance = 20.0f;
    private float SightDistance = 20.0f;
    private float attackTraveled = 0.0f;

    void Start ()
    {
        initialPosition = transform.position;
        playerFollowers = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFollowers>();
    }
	
	void Update ()
    {
        var moveDir = 0;
        switch (state)
        {
            case State.ATTACK:
                moveDir = 1;
                target = playerFollowers.GetTarget().position;
                if (attackTraveled >= AttackDistance)
                    state = State.RETRACT;
                break;
            case State.RETRACT:
                target = initialPosition;
                moveDir = -1;
                if (Vector3.Distance(transform.position, target) < 1.0f)
                    state = State.IDLE;
                break;
            case State.IDLE:
                target = playerFollowers.GetTarget().position;
                if (Vector3.Distance(transform.position, target) < SightDistance)
                {
                    state = State.ATTACK;
                    attackTraveled = 0.0f;
                }
                break;
        }

        var newPosition = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        var movement = newPosition - transform.position;
        attackTraveled += movement.magnitude; //Only used by attackState

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, moveDir * movement, 0.1f, 0));
        transform.position += moveDir * transform.forward * movement.magnitude;
    }

    //Quite similar to Enemy script
    public void OnTriggerEnter(Collider col)
    {
        if (state != State.ATTACK)
            return;
        if (col.transform.tag == "Follower")
        {
            col.transform.parent.GetComponent<FollowFish>().Respawn();
            state = State.RETRACT;
        }
        else if (col.transform.tag == "Player")
        {
            col.transform.GetComponent<Player>().Kill(Player.DeathCause.EATEN);
        }
    }
}
