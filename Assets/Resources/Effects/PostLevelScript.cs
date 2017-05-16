using UnityEngine;
using System.Collections;

public class PostLevelScript : MonoBehaviour
{
    private enum State {BEFORE, ZOOMOUT, SCORING, DONE };
    [Header("Post Level Effect Controls")]
    public Light DirectionaLight;
    public float ZoomSpeed = 0.33f;
    public float SpinSpeed = 30;
    public float ScoreDelay = 5;
    [Tooltip("Seconds from when it's done counting up your score to when the next level starts")]
    public float NextLevelDelay = 3;
    [Header("Don't Change")]
    public Transform FilterTarget;
    public Transform PanOutTarget;
    public Transform PanOutLookAt;
    public GameObject postLevelCanvas;

    private UnityStandardAssets.Cameras.AutoCam cam;
    private CameraController camCon;
    private Fog fog;
    private State state;
    private float timer = 0;
    private PostLevelCanvas canvas;

    // Use this for initialization
    void Start ()
    {
        state = State.BEFORE;
        fog = Camera.main.GetComponent<Fog>();
        cam = Camera.main.transform.parent.GetComponentInParent<UnityStandardAssets.Cameras.AutoCam>();
        camCon = Camera.main.GetComponentInParent<CameraController>();
        canvas = Instantiate(postLevelCanvas).GetComponent<PostLevelCanvas>();
    }
	
	// Update is called once per frame
	void Update () {
	    switch (state)
	    {
	            case State.ZOOMOUT:
	                timer += Time.deltaTime;
	                if (timer > 3)
	                {
	                    cam.SetTarget(PanOutTarget);
	                    cam.SetSpeed(ZoomSpeed);
	                    float step = SpinSpeed * Time.deltaTime;
	                    FilterTarget.position = Vector3.MoveTowards(FilterTarget.position, PanOutLookAt.position, step);
	                    DirectionaLight.intensity = 1;
	                }
	                if (timer > ScoreDelay)
	                    canvas.Activate(this);
	                if (timer > ScoreDelay && FilterTarget.position == PanOutLookAt.position)
                {
                    state = State.SCORING;
                    timer = 0;
                }
	                break;

                    case State.DONE:
                        timer += Time.deltaTime;
                if (timer > NextLevelDelay)
                    World.i.NextLevel();
	                break;
        }
	}

    public void Activate()
    {
        fog.state = Fog.State.DISSIPATE;
        camCon.Player = FilterTarget.gameObject;
        state = State.ZOOMOUT;
    }

    public void Done()
    {
        state=State.DONE;
    }
}
