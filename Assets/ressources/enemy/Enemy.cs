using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State { PATROL, INSIGHT, OUTOFSIGHT };
    private int curPatTarget = -1;
    private Vector3 target;
    private float velocity = 1;

    private State state;
    private GameObject player;
    private List<Vector3> patrolPath = new List<Vector3>(); //Found from game hierachy
    private Quaternion initialRotation;
    private Animator animator;

    public float SightRange = 20;
    public float chaseVelocity = 6, pathVelocity = 4;
    public float Radius = 2;
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform.position;
        initialRotation = transform.rotation;
        animator = GetComponent<Animator>();

        Navigator nav = GameObject.FindGameObjectWithTag("Navigator").GetComponent<Navigator>();


        Transform path = transform.parent.FindChild("Path");
        Vector3[] milestones = new Vector3[path.childCount + 1];
        milestones[0] = transform.position;
        for (int i = 0; i < path.childCount; i++)
            milestones[i + 1] = path.GetChild(i).transform.position;

        for(int i = 0; i < milestones.Length; i++)
        {
            Vector3 from = milestones[i];
            Vector3 to = milestones[(i + 1) % milestones.Length];
            List<Vector3> localPath;
            if (nav.TryFindPath(from, to, out localPath))
                patrolPath.AddRange(localPath);
            else
                Debug.Log("Path not found", this);
        }
        state = State.PATROL;
        SetPathPoint();
    }

    void SetPathPoint()
    {
        target = patrolPath[++curPatTarget % patrolPath.Count];
    }

    protected void FixedUpdate()
    {
        switch(state)
        {
            case State.PATROL:
                if((transform.position - target).magnitude < 0.01f)
                    SetPathPoint();
                velocity = pathVelocity;
                if(CheckSight())
                    state = State.INSIGHT;
                break;
            case State.INSIGHT:
                target = player.transform.position;
                velocity = chaseVelocity;
                if(!CheckSight()) {
                    state = State.PATROL;
                    target = patrolPath[curPatTarget % patrolPath.Count];
                }
                break;
            case State.OUTOFSIGHT:
                break;
        }

        if(animator != null)
            animator.SetBool("IsSwimming", velocity > 0.02f);
        transform.LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target, velocity * Time.deltaTime);
    }

    protected bool CheckSight()
    {
        Vector3 direction = player.transform.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * SightRange, Color.red, 0.1f);
        if(Physics.SphereCast(ray.origin, Radius,ray.direction, out hit, SightRange, int.MaxValue)) //Figure out layers
            if(hit.transform.tag == "Player")
                return true;
        return false;
    }
}
