using UnityEngine;
using System.Collections.Generic;

public class SeeSawPuzzle : MonoBehaviour {
    private enum State { IDLE, BEFORE, DOING, AFTER };
    private State state = State.IDLE;
    public List<Rigidbody> bodies;
    public Transform newTarget;
    public UnityStandardAssets.Cameras.AutoCam cam;
    public CameraController camCon;
    float timer = 0;

    void OnTriggerEnter(Collider col)
    {
        if(state != State.IDLE || !(col.tag == "Player" || (col.transform.parent != null && col.transform.parent.tag == "Player")))
            return;
        cam.SetTarget(newTarget);
        camCon.Player = newTarget.gameObject;
        state = State.BEFORE;
    }

    void Update() {
        switch(state)
        {
            case State.BEFORE:
                timer += Time.deltaTime;
                if (timer > 2.0f)
                {
                    foreach(Rigidbody r in bodies)
                        r.isKinematic = false;
                    timer = 0;
                    state = State.DOING;
                }
                break;
            case State.DOING:
                timer += Time.deltaTime;
                if (timer > 6.2f)
                {
                    GameObject g = GameObject.FindGameObjectWithTag("Player");
                    cam.SetTarget(g.transform);
                    camCon.Player = g;
                    state = State.AFTER;
                    timer = 0;
                }
                break;
        }
    }
}
