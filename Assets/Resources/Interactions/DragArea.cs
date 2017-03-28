using UnityEngine;
using System.Collections.Generic;

public class DragArea : MonoBehaviour {
    private enum State { DRAG, NODRAG};
    private State state;
    public float MaxDrag = 10, BaseDrag = 3, DragMultiplier = 0.5f, EnemyMultiplier = 2;
    private List<GameObject> dragables;
    public Transform dragPoint;
    public GameObject particles;
    private Vector3 halfScale;


    void Start()
    {
        state = State.DRAG;
        halfScale = transform.localScale * 0.5f;
        dragables = new List<GameObject>();
        foreach (Transform t in particles.transform)
        {
            dragables.Add(t.gameObject);
            t.position = RandomPos();
        }
    }

    void FixedUpdate () {
        if(state == State.NODRAG)
            return;
        foreach(GameObject g in dragables) {
            Transform t = g.transform;
            float mult = (g.tag == "Enemy") ? EnemyMultiplier : 1;
            float invDist = Mathf.Max(0, (MaxDrag - Vector3.Distance(t.position, dragPoint.position))) + BaseDrag;
            invDist = Mathf.Min(invDist, MaxDrag) * EnemyMultiplier * DragMultiplier;
            t.position = Vector3.MoveTowards(t.position, dragPoint.position, invDist * Time.deltaTime);
            if (t.position == dragPoint.position && t.tag == "dragable") { 
                t.transform.position = RandomPos();
                t.GetComponent<Billboard>().Spawn();
            }
        }
    }

    private Vector3 RandomPos()
    {
        Vector3 pos = dragPoint.position;
        while(Vector3.Distance(pos, dragPoint.position) < 10) {
            pos = transform.position + new Vector3(Random.Range(-halfScale.x, halfScale.x),
                                                                        Random.Range(-halfScale.y, halfScale.y),
                                                                        Random.Range(-halfScale.z, halfScale.z));
        }
        return pos;
    }

    public void StopDrag(){
        state = State.NODRAG;
        foreach(Transform t in transform)
            Destroy(t.gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy" || col.tag == "Player")
            dragables.Add(col.gameObject);
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Enemy" || col.tag == "Player")
            dragables.Remove(col.gameObject);
    }
}
