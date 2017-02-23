using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    enum State { PATROL, INSIGHT, OUTOFSIGHT };
    private Vector3[] patrolPath; //Found from game hierachy
    private int curPatTarget = -1;
    private State state;
    private GameObject player;
    private Vector3 target;
    private float velosity = 1;

    public float SightRange = 20;
    public float chaseVelosity = 6, pathVelosity = 4;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform.position;
        Transform path = transform.parent.FindChild("Path");
        patrolPath = new Vector3[path.childCount];
        int i = 0;
        foreach (Transform t in path)
            patrolPath[i++] = t.position;
        state = State.PATROL;
        SetPathPoint();
    }

    void SetPathPoint()
    {
        target = patrolPath[++curPatTarget % patrolPath.Length];
    }

    void FixedUpdate () {
        switch(state)
        {
            case State.PATROL:
                if(transform.position == target)
                    SetPathPoint();
                velosity = pathVelosity;
                if(CheckSight())
                    state = State.INSIGHT;
                break;
            case State.INSIGHT:
                if(!CheckSight())
                    state = State.PATROL;
                target = player.transform.position;
                velosity = chaseVelosity;
                break;
            case State.OUTOFSIGHT:

                break;
        }
        transform.LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target, velosity * Time.deltaTime);
    }

    private bool CheckSight()
    {
        Vector3 direction = player.transform.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * SightRange, Color.red, 0.1f);
        if(Physics.Raycast(ray.origin, ray.direction, out hit, SightRange, int.MaxValue)) //Figure out layers
            if(hit.transform.tag == "Player")
                return true;
        return false;
    }
}
