using UnityEngine;

public class Moray : MonoBehaviour
{
    public GameObject MoraneSegment;
    public enum MorayState {ATTACK, RETRACT, IDLE};
    [HideInInspector]
    public MorayState State = MorayState.IDLE;
    public Vector3 InitialPosition, InitialForward;
    private PlayerFollowers playerFollowers;
    private Vector3 target;
    private Quaternion initialRotation;

    public float Speed = 10.0f;
    public float AttackDistance = 20.0f;
    public float SightDistance = 20.0f, SightAngle = 100;
    private float attackTraveled = 0.0f;
    private float segmentTraveled = 0.0f;
    private float segmentSize;
    private MoraySegment HeadSegment, TailSegment;

    public float lastDist = float.PositiveInfinity;
    void Start ()
    {
        InitialPosition = transform.position;
        initialRotation = transform.rotation;
        InitialForward = transform.forward;
        playerFollowers = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFollowers>();
        segmentSize = MoraneSegment.transform.localScale.z * 0.9f;
    }
	
	void Update ()
    {
        var moveDir = 0;
        switch (State)
        {
            case MorayState.ATTACK:
                moveDir = 1;
                target = playerFollowers.GetTarget().position;

                while(segmentTraveled >= 1.0f)
                {
                    var ray = new Ray(transform.position, transform.forward);
                    RaycastHit hit;
                    if (attackTraveled >= AttackDistance ||
                        (Physics.Raycast(ray.origin, ray.direction, out hit, 1.5f, 1) && hit.transform.tag != "Player"))
                    {
                        State = MorayState.RETRACT;
                        return;
                    }

                    var g = (GameObject)Instantiate(MoraneSegment, InitialPosition - 2.5f * InitialForward, initialRotation);
                    var segment = g.GetComponent<MoraySegment>();
                    segment.Moray = this;
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
                    //segment.InnerUpdate(segmentTraveled / Speed);
                    segmentTraveled -= 1.0f;
                    TailSegment = segment;
                }
                break;
            case MorayState.RETRACT:
                target = (HeadSegment == null) ? InitialPosition : HeadSegment.transform.position;
                moveDir = -1;
                float newDist = Vector3.Distance(transform.position, InitialPosition);
                if (newDist > lastDist && HeadSegment == null)
                {
                    transform.position = InitialPosition;
                    transform.rotation = initialRotation;
                    State = MorayState.IDLE;
                    lastDist = float.PositiveInfinity;
                }
                lastDist = newDist;
                break;
            case MorayState.IDLE:
                target = playerFollowers.GetTarget().position;
                if (Vector3.Distance(transform.position, target) < SightDistance && 
                    Vector3.Angle(transform.forward, target - transform.position) < SightAngle)
                {
                    State = MorayState.ATTACK;
                    attackTraveled = 0.0f;
                    segmentTraveled = 0.8f;
                }
                break;
        }

        var newPosition = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        var movement = newPosition - transform.position;
        attackTraveled += movement.magnitude; //Only used by attackState
        segmentTraveled += movement.magnitude; //Only used by attackState
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, moveDir * movement, 0.4f, 0));
        transform.position += moveDir * transform.forward * movement.magnitude;
    }
    
    public void OnTriggerEnter(Collider col)
    {
        if (State != MorayState.ATTACK)
            return;

        Debug.Log(col.transform.tag);
        if (col.transform.tag == "Follower")
        {
            col.transform.parent.GetComponent<FollowFish>().Respawn();
            State = MorayState.RETRACT;
        }
        //If player is accedentaly eaten often this should include a transform test
        else if (col.transform.tag == "Player")
        {
            col.transform.GetComponent<Player>().Kill(Player.DeathCause.EATEN);
            State = MorayState.RETRACT;
        }
    }
}
