using UnityEngine;
using System.Collections;
using UnityStandardAssets.Cameras;

public class Level2End : MonoBehaviour
{
    public SpringJoint[] weed;
    public FixedJoint stuckWeed;
    public HingeJoint finalWeed;
    public PostLevelScript postLevel;
    private AutoCam cam;
    private CameraController camCon;
    public Transform lookFrom;
    public Enemy EndEnemy;
    public DragArea EndDragArea;
    public Transform DragPoint;
    public Transform FilterPoint;
    public DragArea FilterDragArea;
    public GameObject Hole;
    private ParticleSystem[] waterSpray;
    private bool running = false;
    private bool disabledWaterHole = false;
    private float timer = 0;

	// Use this for initialization
	void Start ()
	{
	    cam = Camera.main.transform.parent.GetComponentInParent<AutoCam>();
        camCon = Camera.main.GetComponentInParent<CameraController>();
	    waterSpray = Hole.GetComponentsInChildren<ParticleSystem>();
        Run();
    }
	
	// Update is called once per frame
	void Update () {
	    if (!running) return;
	    if (!disabledWaterHole)
	    {
	        if (EndDragArea.GetState() == DragArea.State.NODRAG)
	        {
	            foreach (var f in waterSpray)
	            {
                    f.Stop();
	            }
	            Hole.GetComponentInChildren<AudioSource>().Stop();
	            disabledWaterHole = true;
	        }
        }

	    timer += Time.deltaTime;

	    if (!(timer > 2)) return;

	    float step = 30 * Time.deltaTime;
	    DragPoint.position = Vector3.MoveTowards(DragPoint.position, FilterPoint.position, step);
	    lookFrom.position = Vector3.MoveTowards(lookFrom.position, FilterPoint.position, step/4);
	    FilterDragArea.DragMultiplier = Mathf.Lerp(1, 36, (timer-2)/5);

	    if (!(timer > 7)) return;

	    postLevel.Activate();
	    Destroy(gameObject);
	}

    public void Run()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SwimAway();
        EndEnemy.SetFrozen();
        EndEnemy.transform.rotation = transform.rotation;
        EndDragArea.IgnorePlayer();
        Destroy(stuckWeed);
        foreach (SpringJoint j in weed)
        {
            j.GetComponent<CapsuleCollider>().isTrigger = false;
            j.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(j);
        }
        finalWeed.GetComponent<CapsuleCollider>().isTrigger = false;
        Destroy(finalWeed);
        Destroy(weed[0].gameObject);
        camCon.Player = DragPoint.gameObject;
        cam.SetTarget(lookFrom);
        running = true;
        //postLevel.Activate();
        //Destroy(gameObject);
    }
}
