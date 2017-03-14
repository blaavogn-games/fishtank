using UnityEngine;
using System.Collections;

public class FullShell : MonoBehaviour {
    public float animationTimeout = 5.0f;

	private float animationTimer = 0.0f;
	private Animator animator;

	void Start ()
    {
		animator = GetComponent<Animator>();
	}
	
    void Toggle() 
    {
        bool isOpen = animator.GetBool("IsOpen");

        animator.SetBool("IsOpen", !isOpen);

        animationTimer = animationTimeout;
    }

	void Update ()
    {
		if(animationTimer <= 0.0f)
		{
            Toggle();
		}
        else
        {
            animationTimer -= Time.deltaTime;
        }
	}
}
