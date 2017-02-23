using UnityEngine;
using System.Collections.Generic;

public class PathFollower : MonoBehaviour {
    Navigator navigator;
    public float Velosity = 1;
    public bool HasPath { get; private set;}
    private Stack<Vector3> path;
    private Vector3 target;

    void Start () {
        navigator = GameObject.FindGameObjectWithTag("Navigator").GetComponent<Navigator>();
        FindPath();
    }

    void FindPath()
    {
        HasPath = navigator.TryFindPath(new Vector3(0,0,0), new Vector3(4,4,4), out path);
        target = path.Pop();
    }

    void FollowPath() //Inline for Blow?!
    {
        if(!HasPath)
            return;
        if(target == transform.position)
        {
            if(path.Count > 0)
                target = path.Pop();
            else {
                HasPath = false;
                return;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, target, Velosity * Time.deltaTime);
    }

    void Update () {
        FollowPath();
    }
}
