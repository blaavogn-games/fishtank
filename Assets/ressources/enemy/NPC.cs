using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/**
 * A main class from which NPC types can inherit
 */
public class NPC : MonoBehaviour {
    [Serializable]
    public enum State {
        Idle,
        Patrolling,
        Following
    };

    public State state = State.Idle;
    public float turningSpeed = 1.0f;
    public float movementSpeed = 8.0f;
    public float accelerationSpeed = 5.0f;
    public float stoppingDistance = 0.25f;
    public float slowingDistance = 1.0f;
    public float slowingMovementSpeed = 1.0f;
    public float visionRange = 20.0f;
    public float attentionSpan = 5.0f;
    public LayerMask visionLayerMask = 1;
    public LayerMask avoidanceLayerMask = 1;

    private int currentPatrolPoint = 0;
    private float currentMovementSpeed = 0.0f;
    private List<Transform> patrolPoints = new List<Transform>();
    private GameObject player;
    private Vector3 target;
    private Animator animator;
    private Vector3 initialPosition; 
    private Quaternion initialRotation; 
    private State initialState;
    private float attentionTimer = 0.0f;
    private List<Vector3> followPoints = new List<Vector3>();
    private float followPointUpdateTimer = 0.0f;

    private const float FOLLOW_POINT_UPDATE_INTERVAL = 1.0f;
    private const float FOLLOW_POINT_MIN_DISTANCE = 2.0f;
    private const int LOCAL_AVOIDANCE_MAX_ATTEMPTS = 20;
    private const float LOCAL_AVOIDANCE_ATTEMPT_ANGLE = 5.0f;
    private const float LOCAL_AVOIDANCE_RAYCAST_DISTANCE = 3.0f;

    /**
     * Initialise
     */
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialState = state;

        animator = GetComponent<Animator>();

        player = GameObject.FindWithTag("Player");

        Transform pathParent = transform.parent.FindChild("Path");

        if(pathParent != null)
        {
            foreach(Transform pathNode in pathParent) 
            {
                patrolPoints.Add(pathNode);
            }
        }

        SetState(state);
    }

    /**
     * Picks the next patrol point in the array
     */
    void GoToNextPatrolPoint() 
    {
        if (patrolPoints.Count == 0) { return; }

        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Count;
    
        target = patrolPoints[currentPatrolPoint].position;
    }

    /**
     * Linearly interpolate towards a rotation
     *
     * @param {Quaternion} targetRotation
     */
    void Rotate(Quaternion targetRotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turningSpeed);
    }

    /**
     * Moves towards the target
     */
    void MoveTowardsTarget()
    {
        Vector3 direction = (target - transform.position).normalized;
       
        // Local avoidance
        // TODO: Finish this
        //bool foundOpenDirection = false;
        //int attempts = 0;
        // 
        //Ray ray = new Ray(transform.position, direction);
        //RaycastHit hit;
        //
        //while(!foundOpenDirection && attempts < LOCAL_AVOIDANCE_MAX_ATTEMPTS)
        //{
        //    if(Physics.Raycast(ray.origin, ray.direction * LOCAL_AVOIDANCE_RAYCAST_DISTANCE, out hit, LOCAL_AVOIDANCE_RAYCAST_DISTANCE, avoidanceLayerMask))
        //    {
        //        direction = Quaternion.AngleAxis(LOCAL_AVOIDANCE_ATTEMPT_ANGLE, Vector3.up) * direction;
        //        attempts++;
        //    }
        //    else
        //    {
        //        foundOpenDirection = true;
        //    }
        //}

        // Rotate towards target and follow it
        Rotate(Quaternion.LookRotation(direction));
      
        transform.position = transform.position + direction * currentMovementSpeed * Time.deltaTime;

        Debug.DrawLine(transform.position, target);
    }

    /**
     * Updates the current movement speed
     */
    void UpdateCurrentMovementSpeed()
    {
        if(IsWithinSlowingDistance())
        {
            currentMovementSpeed = Mathf.SmoothStep(currentMovementSpeed, slowingMovementSpeed, Time.deltaTime * accelerationSpeed);
        }
        else
        {
            currentMovementSpeed = Mathf.SmoothStep(currentMovementSpeed, movementSpeed, Time.deltaTime * accelerationSpeed);
        }
    }

    /**
     * Gets the distance to the target
     *
     * @returns {float} Distance
     */
    float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, target);
    }

    /**
     * Checks if within stopping distance
     *
     * @returns {bool} Is within stopping distance
     */
    bool IsWithinStoppingDistance()
    {
        return GetDistanceToTarget() < stoppingDistance;
    }
    
    /**
     * Checks if within slowing distance
     *
     * @returns {bool} Is within slowing distance
     */
    bool IsWithinSlowingDistance()
    {
        return GetDistanceToTarget() < slowingDistance;
    }

    /**
     * Checks if can see the player
     *
     * @returns {bool} Can see player
     */
    bool CanSeePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        bool sawPlayer = false;
        bool foundHit = Physics.Raycast(ray.origin, ray.direction * visionRange, out hit, visionRange, visionLayerMask);

        if(foundHit)
        {
            if(hit.transform.tag == "Player")
            {
                sawPlayer = true;
            }
        }
        
        Debug.DrawLine(transform.position, (foundHit ? hit.transform.position : transform.position + ray.direction * visionRange), (sawPlayer ? Color.green : Color.red), 0.1f);
       
        return sawPlayer;
    }

    /**
     * Updates the patrolling behaviour
     */
    void UpdatePatrolling()
    {
        if(IsWithinStoppingDistance())
        {
            GoToNextPatrolPoint();
        }

        UpdateCurrentMovementSpeed();
        MoveTowardsTarget();
        animator.SetBool("IsSwimming", true);

        if(CanSeePlayer())
        {
            SetState(State.Following);
        }
    }
    
    /**
     * Updates the following behaviour
     */
    void UpdateFollowing()
    {
        
        UpdateCurrentMovementSpeed();
        MoveTowardsTarget();
        animator.SetBool("IsSwimming", true);
   
        // Update attention timer based on whether or not we can see the player
        if(CanSeePlayer())
        {
            attentionTimer = attentionSpan;

            target = player.transform.position;

            // Make sure the follow path is cleared
            followPointUpdateTimer = 0.0f;

            if(followPoints.Count > 0)
            {
                followPoints.Clear();
            }
        }

        // If we can't see the player, update the following path
        else
        {
            // Add a follow point if needed
            if(followPointUpdateTimer <= 0.0f)
            {
                // Only add a new point if the we can't see the player and
                // the last point on the list isn't too close to the player
                if(followPoints.Count < 1 || Vector3.Distance(followPoints[followPoints.Count - 1], player.transform.position) > FOLLOW_POINT_MIN_DISTANCE)
                {
                    followPoints.Add(player.transform.position);
                }

                followPointUpdateTimer = FOLLOW_POINT_UPDATE_INTERVAL;
            }

            // Decrement the follow point timer
            else
            {
                followPointUpdateTimer -= Time.deltaTime;
            }
        
            // Set target based on follow points
            if(followPoints.Count > 0)
            {
                target = followPoints[0];

                if(Vector3.Distance(transform.position, target) < stoppingDistance)
                {
                   followPoints.RemoveAt(0); 
                }
            }


            // Decrement attention timer
            attentionTimer -= Time.deltaTime;

            if(attentionTimer <= 0.0f)
            {
                SetState(initialState);
            }
        }
    }

    /**
     * Updates the idle behaviour
     */
    void UpdateIdle()
    {
        target = initialPosition;
        
        UpdateCurrentMovementSpeed();
        
        if(!IsWithinStoppingDistance())
        {
            MoveTowardsTarget();
            animator.SetBool("IsSwimming", true);
        }
        else
        {
            Rotate(initialRotation);
            animator.SetBool("IsSwimming",false);
        }
        
        if(CanSeePlayer())
        {
            SetState(State.Following);
        }
    }

    /**
     * Sets the state
     *
     * @param {State} newState
     */
    void SetState(State newState)
    {
        state = newState;
        
        switch(state)
        {
            case State.Patrolling:
                GoToNextPatrolPoint();
                break;

            case State.Following:
                followPoints.Clear();
                break;
        }
    }

    /**
     * Called every frame
     */
    void Update()
    {
        switch(state)
        {
            case State.Idle:
                UpdateIdle();
                break;
            
            case State.Following:
                UpdateFollowing();
                break;
            
            case State.Patrolling:
                UpdatePatrolling();
                break;
        }
    }
}
