using UnityEngine;

public class Murane : MonoBehaviour
{
    public GameObject MoraneSegment;
    public enum MuraneState {ATTACK, RETRACT, IDLE};
    public MuraneState State = MuraneState.ATTACK;
    public Vector3 InitialPosition;
    private PlayerFollowers playerFollowers;
    private Vector3 target;
    private Quaternion initialRotation;

    public float Speed = 10.0f;
    private float AttackDistance = 20.0f;
    private float SightDistance = 20.0f, SightAngle = 70;
    private float attackTraveled = 0.0f;
    private float segmentTraveled = 0.0f;
    private float segmentSize;
    private MuraneSegment HeadSegment, TailSegment;

    void Start ()
    {
        InitialPosition = transform.position;
        initialRotation = transform.rotation;
        playerFollowers = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFollowers>();
        segmentSize = MoraneSegment.transform.localScale.z * 0.9f;
    }
	
	void Update ()
    {
        var moveDir = 0;
        switch (State)
        {
            case MuraneState.ATTACK:
                moveDir = 1;
                target = playerFollowers.GetTarget().position;
                if (segmentTraveled >= 1.0f)
                {
                    segmentTraveled = 0.0f;
                    var g = (GameObject)Instantiate(MoraneSegment, InitialPosition, initialRotation);
                    var segment = g.GetComponent<MuraneSegment>();
                    segment.Murane = this;
                    if (TailSegment != null)
                    {
                        TailSegment.Tail = segment.transform;
                        segment.Head = TailSegment.transform;
                    }
                    else
                    {
                        HeadSegment = segment;
                        segment.Head = transform;
                    }
                    TailSegment = segment;
                }
                if (attackTraveled >= AttackDistance)
                    State = MuraneState.RETRACT;
                break;
            case MuraneState.RETRACT:
                target = (HeadSegment == null) ? InitialPosition : HeadSegment.transform.position;
                moveDir = -1;
                if (Vector3.Distance(transform.position, InitialPosition) < 1.0f)
                {
                    InitialPosition = transform.position;
                    initialRotation = transform.rotation;
                    State = MuraneState.IDLE;
                }
                break;
            case MuraneState.IDLE:
                target = playerFollowers.GetTarget().position;
                if (Vector3.Distance(transform.position, target) < SightDistance && 
                    Vector3.Angle(transform.position, target) < SightAngle * Mathf.Deg2Rad)
                {
                    Debug.Log(Vector3.Angle(transform.position, target) * Mathf.Rad2Deg);
                    State = MuraneState.ATTACK;
                    attackTraveled = 0.0f;
                    segmentTraveled = 0.0f;
                }
                break;
        }

        var newPosition = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        var movement = newPosition - transform.position;
        attackTraveled += movement.magnitude; //Only used by attackState
        segmentTraveled += movement.magnitude; //Only used by attackState

        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, moveDir * movement, 0.1f, 0));
        transform.position += moveDir * transform.forward * movement.magnitude;
    }
    
    public void OnTriggerEnter(Collider col)
    {
        if (State != MuraneState.ATTACK)
            return;
        if (col.transform.tag == "Follower")
        {
            col.transform.parent.GetComponent<FollowFish>().Respawn();
            State = MuraneState.RETRACT;
        }
        //If player is accedentaly eaten often this should include a transform test
        else if (col.transform.tag == "Player")
        {
            col.transform.GetComponent<Player>().Kill(Player.DeathCause.EATEN);
            State = MuraneState.RETRACT;
        }
    }
}
