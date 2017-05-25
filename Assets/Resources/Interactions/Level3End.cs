using UnityEngine;
using System.Collections;

public class Level3End : MonoBehaviour
{
    public enum State
    {
        IDLE,
        ONPLAYER,
        ONMORAY
    };
    public SpringJoint[] Weed;
    public Transform CameraTarget;
    public GameObject CameraPlayer;
    public GameObject FilterTarget;
    public Transform OutOfCaveTarget;
    public Behaviour halo;
    public DragArea FilterDragArea;


    private State state = State.IDLE;

    private HingeJoint joint;
    private Rigidbody body;
    private Rigidbody playerBody;
    private Player player;
    private UnityStandardAssets.Cameras.AutoCam cam;
    private CameraController camCon;
    private float timer = 0;
    
    public PostLevelScript PostLevel;

    // Use this for initialization
    void Start()
    {
        joint = GetComponentInParent<HingeJoint>();
        body = GetComponentInParent<Rigidbody>();
        playerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        camCon = Camera.main.GetComponentInParent<CameraController>();
        cam = Camera.main.transform.parent.GetComponentInParent<UnityStandardAssets.Cameras.AutoCam>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.ONMORAY)
        {
            timer += Time.deltaTime;
            FilterDragArea.DragMultiplier = Mathf.Lerp(1, 5, timer/5);
            float step = 5f * Time.deltaTime;
            CameraTarget.position = Vector3.MoveTowards(CameraTarget.position, OutOfCaveTarget.position, step);
            if (timer > 3)
            {
                CameraPlayer.transform.position = Vector3.MoveTowards(CameraPlayer.transform.position,
                    FilterTarget.transform.position, step * 5);
            }
            if (Vector3.Distance(CameraTarget.position, OutOfCaveTarget.position)<1)
            {
                PostLevel.Activate();
                Destroy(gameObject);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (state == State.IDLE && other.attachedRigidbody == playerBody)
        {
            halo.enabled = false;
            transform.position = player.transform.position;
            transform.parent = player.transform;
            state = State.ONPLAYER;
            joint.connectedBody = playerBody;
            body.isKinematic = false;
        }
        else if (state == State.ONPLAYER && other.GetComponent<Moray>() != null)
        {
            transform.parent = null;
            foreach (SpringJoint j in Weed)
            {
                j.GetComponent<CapsuleCollider>().isTrigger = false;
                j.breakForce = 0;
            }
            Camera.main.GetComponent<Fog>().state = Fog.State.DISSIPATE;
            Destroy(Weed[0].gameObject);
            state = State.ONMORAY;
            joint.connectedBody = other.transform.parent.GetComponent<Rigidbody>();
            other.GetComponent<Moray>().State = Moray.MorayState.EATEN;
            CameraPlayer.transform.position = other.transform.parent.position;
            camCon.Player = CameraPlayer;
            CameraTarget.transform.position = player.transform.position;
            cam.SetTarget(CameraTarget);
            player.transform.position = other.transform.parent.position;
            player.FakeDeath();
        }
    }
}
