using UnityEngine;
using System.Collections;
using UnityStandardAssets.Cameras;

public class Level2End : MonoBehaviour
{
    public SpringJoint[] weed;
    public FixedJoint stuckWeed;
    public PostLevelScript postLevel;
    private AutoCam cam;
    private CameraController camCon;
    public Transform lookFrom;
    public Enemy EndEnemy;
    public DragArea EndDragArea;
    public Transform DragPoint;
    public Transform FilterPoint;
    private bool running = false;
    private float timer = 0;

	// Use this for initialization
	void Start ()
	{
	    cam = Camera.main.transform.parent.GetComponentInParent<AutoCam>();
        camCon = Camera.main.GetComponentInParent<CameraController>();
        Run();
    }
	
	// Update is called once per frame
	void Update () {
	    if (running)
	    {
	        timer += Time.deltaTime;
            if (timer > 2)
            {
                float step = 30 * Time.deltaTime;
                DragPoint.position = Vector3.MoveTowards(DragPoint.position, FilterPoint.position, step);
                if (timer > 5)
                {
                    postLevel.Activate();
                    Destroy(gameObject);
                }
            }

        }
	}

    public void Run()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Freeze();
        EndEnemy.SetFrozen();
        EndEnemy.transform.rotation = transform.rotation;
        EndDragArea.IgnorePlayer();
        Destroy(stuckWeed);
        foreach (SpringJoint j in weed)
        {
            Destroy(j);
        }
        camCon.Player = DragPoint.gameObject;
        cam.SetTarget(lookFrom);
        running = true;
        //postLevel.Activate();
        //Destroy(gameObject);
    }
}
