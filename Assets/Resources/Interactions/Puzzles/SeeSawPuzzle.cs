using UnityEngine;
using System.Collections.Generic;

public class SeeSawPuzzle : MonoBehaviour {
    private enum State { IDLE, BEFORE, DOING, FILTERDRAGGING, AFTER };
    private State state = State.IDLE;
    public List<Rigidbody> bodies;
    public Transform newTarget;
    public GameObject boulder;
    public UnityStandardAssets.Cameras.AutoCam cam;
    public CameraController camCon;
    public Transform FilterTarget;
    public Transform PanOutTarget;
    float timer = 0;

    void OnTriggerEnter(Collider col)
    {
        if(state != State.IDLE || col.tag != "Player")
            return;
        cam.SetTarget(newTarget);
        camCon.Player = boulder;
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
                    //GameObject g = GameObject.FindGameObjectWithTag("Player");
                    //cam.SetTarget(FilterTarget);
                    camCon.Player = FilterTarget.gameObject;
                    //FilterDragArea.DragMultiplier += 10;
                    state = State.FILTERDRAGGING;
                    timer = 0;
                }
                break;
            case State.FILTERDRAGGING:
                break;
        }
    }
}
