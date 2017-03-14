using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State { PATROL, INSIGHT, OUTOFSIGHT, IDLE };
    private int curPatTarget = -1;
    private Vector3 target;
    private float velocity = 1;

    protected State state;
    protected GameObject player;
    protected List<Vector3> patrolPath = new List<Vector3>(); //Found from game hierachy
    protected Quaternion initialRotation;
    protected Animator animator;

    public float idlePositionThreshold = 3f;
    public float SightRange = 20;
    public float chaseVelocity = 6, pathVelocity = 4;

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
            Vector3 to = milestones[(i + i) % path.childCount];
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

    void SetSwimming(bool isSwimming)
    {
        if(animator != null)
        {
            animator.SetBool("IsSwimming", isSwimming);
        }
    }

    protected void UpdateMotion()
    {
        SetSwimming(true);

        switch(state)
        {
            case State.IDLE:
                target = patrolPath[0];
                velocity = pathVelocity;
                transform.position = Vector3.MoveTowards(transform.position, target, velocity * Time.deltaTime);
                
                if(Vector3.Distance(transform.position, target) <= idlePositionThreshold)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime);

                    SetSwimming(false);
                }
                else
                {
                    transform.LookAt(target);
                }
                return;

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

        transform.LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target, velocity * Time.deltaTime);
    }

    void FixedUpdate ()
    {
        UpdateMotion();
    }

    protected bool CheckSight()
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
