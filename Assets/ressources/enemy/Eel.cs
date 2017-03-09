using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Eel : Enemy {
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialRotation = transform.rotation;
        animator = GetComponent<Animator>();

        // Set own location as patrol path point, so we won't move
        patrolPath.Add(transform.position);
        
        state = State.IDLE;
	}

    void FixedUpdate() {
        if(CheckSight())
        {
            state = State.INSIGHT;

        }
        else
        {
            state = State.IDLE;
        }

        UpdateMotion();
    }    
}
