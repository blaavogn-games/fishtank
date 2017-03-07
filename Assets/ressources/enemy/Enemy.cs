using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum State { PATROL, INSIGHT, OUTOFSIGHT };
    private List<Vector3> patrolPath; //Found from game hierachy
    private int curPatTarget = -1;
    private State state;
    private GameObject player;
    private Vector3 target;
    private float velosity = 1;

    public float SightRange = 20;
    public float chaseVelosity = 6, pathVelosity = 4;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform.position;

        patrolPath = new List<Vector3>();
        Navigator nav = GameObject.FindGameObjectWithTag("Navigator").GetComponent<Navigator>();

    Transform path = transform.parent.FindChild("Path");
        for(int i = 0; i < path.childCount; i++)
        {
            patrolPath.Add(path.GetChild(i).transform.position);
            Vector3 from = path.GetChild(i).transform.position;
            Vector3 to = path.GetChild((i + i) % path.childCount).transform.position;
            List<Vector3> localPath;
            if (nav.TryFindPath(from, to, out localPath))
                patrolPath.AddRange(localPath);
            else
                Debug.Log("Path not found");
        }
        state = State.PATROL;
        SetPathPoint();
    }

    void SetPathPoint()
    {
        target = patrolPath[++curPatTarget % patrolPath.Count];
    }

    void FixedUpdate ()
    {
        switch(state)
        {
            case State.PATROL:
                if((transform.position - target).magnitude < 0.01f)
                    SetPathPoint();
                velosity = pathVelosity;
                if(CheckSight())
                    state = State.INSIGHT;
                break;
            case State.INSIGHT:
                target = player.transform.position;
                velosity = chaseVelosity;
                if(!CheckSight()) {
                    state = State.PATROL;
                    target = patrolPath[curPatTarget % patrolPath.Count];
                }
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
