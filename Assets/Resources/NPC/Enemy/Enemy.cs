using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State { PATROL, INSIGHT, OUTOFSIGHT, CHARGE, EAT };
    private int curPatTarget = -1;
    private Vector3 target;
    private float velocity = 1;
    private float timer = 0;

    private State state;
    private PlayerFollowers playerFollowers;
    private List<Vector3> patrolPath = new List<Vector3>(); //Found from game hierachy
    private Animator animator;

    public float SightRange = 20;
    public float chaseVelocity = 6, pathVelocity = 4, chargeVelocity = 30;
    public float Radius = 2, ChargeDistance = 6, EatTime = 2;
    void Start ()
    {
        playerFollowers = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFollowers>();
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
            else {
                Debug.Log("Path not found", path.GetChild(i));
            }
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
                if(CheckSight(playerFollowers.GetTarget()))
                    state = State.INSIGHT;
                break;
            case State.INSIGHT:
                target = playerFollowers.GetTarget();
                velocity = Mathf.Lerp(velocity, chaseVelocity, 0.1f);
                if(!CheckSight(target)) {
                    state = State.PATROL;
                    target = patrolPath[curPatTarget % patrolPath.Count];
                }
                if(Vector3.Distance(transform.position,  target) < ChargeDistance)
                    state = State.CHARGE;
                break;
            case State.OUTOFSIGHT:
                break;
            case State.CHARGE:
                target = playerFollowers.GetTarget();
                velocity = Mathf.Lerp(velocity, chargeVelocity, 0.1f);
                //Transition to eat through collision
                break;
            case State.EAT:
                velocity = Mathf.Lerp(velocity, chaseVelocity, 0.1f);
                if(timer < 0)
                    state = State.INSIGHT;
                break;
        }

        timer -= Time.deltaTime;
        if(animator != null)
            animator.SetBool("IsSwimming", velocity > 0.02f);

        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, velocity * Time.deltaTime);
        Vector3 movement = newPosition - transform.position;
        //wiggle.wiggleSpeed = movement.magnitude * 50 + 10;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, movement, 0.05f, 0));
        transform.position += transform.forward * movement.magnitude;
    }

    private bool CheckSight(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * SightRange, Color.red, 0.1f);
        if(Physics.SphereCast(ray.origin, Radius,ray.direction, out hit, SightRange, int.MaxValue)) {
            if(hit.transform.tag == "Player" || hit.transform.tag == "Follower") {
                return true;
             }
        }
        return false;
    }

    public void OnTriggerEnter(Collider col)
    {
        if(state != State.CHARGE)
            return;

        if(col.transform.tag == "Follower")
        {
            col.transform.parent.GetComponent<FollowFish>().Respawn();
            state = State.EAT;
            timer = EatTime;
            target = patrolPath[curPatTarget % patrolPath.Count];
        }
        else if(col.transform.tag == "Player" && target == col.transform.position)
        {
            col.transform.GetComponent<Player>().Kill(Player.DeathCause.EATEN);
        }
    }
}
