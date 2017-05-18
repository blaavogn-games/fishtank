using UnityEngine;
using System.Collections;
using UnityStandardAssets.Cameras;

public class Level2End : MonoBehaviour
{
    public SpringJoint[] weed;
    public Rigidbody stuckWeed;
    public PostLevelScript postLevel;
    private AutoCam cam;
    public Transform lookFrom;

	// Use this for initialization
	void Start ()
	{
	    cam = Camera.main.transform.parent.GetComponentInParent<AutoCam>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Run()
    {
        foreach (SpringJoint j in weed)
        {
            Destroy(j);
        }
        cam.SetTarget(lookFrom);
        postLevel.Activate();
        Destroy(gameObject);
    }
}
