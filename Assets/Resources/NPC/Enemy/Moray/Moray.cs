using UnityEngine;

public class Moray : MonoBehaviour
{
    public GameObject MoraneSegment;
    public enum MorayState {ATTACK, RETRACT, IDLE, EATEN};
    [HideInInspector]
    public MorayState State = MorayState.IDLE;
    public Vector3 InitialPosition, InitialForward;
    private PlayerFollowers playerFollowers;
    private Transform target;
    private Quaternion initialRotation;
    private SoundManager soundManager;
    private AudioSource audioSource;

    public float Speed = 10.0f;
    public float AttackDistance = 20.0f;
    public float SightDistance = 20.0f, SightAngle = 100;
    private float attackTraveled = 0.0f;
    private float segmentTraveled = 0.0f;
    private float segmentSize;
    private MoraySegment HeadSegment, TailSegment;
    
    void Start ()
    {
        InitialPosition = transform.position;
        initialRotation = transform.rotation;
        InitialForward = transform.forward;
        playerFollowers = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFollowers>();
        segmentSize = MoraneSegment.transform.localScale.z * 0.9f;
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        audioSource = GetComponent<AudioSource>();
    }
	
	void Update ()
    {
        var moveDir = 0;
        Vector3 targetPos;
        switch (State)
        {
            case MorayState.ATTACK:
                moveDir = 1;
                target = playerFollowers.GetTarget();
                if (target == null) { 
                    State = MorayState.EATEN;
                    goto case MorayState.EATEN;
                }
                targetPos = target.position;
                while (segmentTraveled >= 1.0f)
                {
                    if (attackTraveled >= AttackDistance)
                    {
                        State = MorayState.EATEN;
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
                targetPos = (HeadSegment == null) ? InitialPosition : HeadSegment.transform.position;
                moveDir = -1;
                if (HeadSegment == null && Vector3.Distance(transform.position, InitialPosition) < 1.0f)
                {
                    transform.position = InitialPosition;
                    transform.rotation = initialRotation;
                    State = MorayState.IDLE;
                }
                break;
            case MorayState.EATEN:
                targetPos = (HeadSegment == null) ? InitialPosition : HeadSegment.transform.position;
                moveDir = -1;
                if (HeadSegment == null && Vector3.Distance(transform.position, InitialPosition) < 1.0f)
                {
                    transform.position = InitialPosition;
                    transform.rotation = initialRotation;
                }
                break;
            default:
            case MorayState.IDLE:
                target = playerFollowers.GetTarget();
                if (target == null) { 
                    State = MorayState.EATEN;
                    goto case MorayState.EATEN;
                }
                targetPos = target.position;
                if (Vector3.Distance(transform.position, targetPos) < SightDistance)
                {
                    State = MorayState.ATTACK;
                    audioSource.PlayOneShot(soundManager.GetClip(SfxTypes.MORRAY_ATTACK));
                    attackTraveled = 0.0f;
                    segmentTraveled = 0.8f;
                }
                targetPos = transform.position;
                break;
        }

        var newPosition = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
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

        if (col.transform.tag == "Follower")
        {
            col.transform.parent.GetComponent<FollowFish>().Respawn();
            State = MorayState.RETRACT;
        }
        //If player is accedentaly eaten often this should include a transform test
        else if (col.transform.tag == "Player" && col.transform == target)
        {
            col.transform.GetComponent<Player>().Kill(Player.DeathCause.EATEN);
            State = MorayState.EATEN;
        }
    }
}
