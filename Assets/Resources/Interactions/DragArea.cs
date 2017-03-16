using UnityEngine;
using System.Collections.Generic;

public class DragArea : MonoBehaviour {
    public float MaxDrag = 10, BaseDrag = 3;
    private List<GameObject> dragables;
    private Vector3 dragPoint; //From hierachy

    void Start()
    {
        dragables = new List<GameObject>();
        foreach(Transform t in transform)
            dragPoint = t.position;
    }

    void FixedUpdate () {
        foreach(GameObject g in dragables) {
            Transform t = g.transform;
            float invDist = Mathf.Max(0, (MaxDrag - Vector3.Distance(t.position, dragPoint))) + BaseDrag;
            invDist = Mathf.Min(invDist, MaxDrag);
            t.position = Vector3.MoveTowards(t.position, dragPoint, invDist * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy" || col.transform.parent != null && col.transform.parent.tag == "Player")
            dragables.Add(col.transform.parent.gameObject);
    }
}
