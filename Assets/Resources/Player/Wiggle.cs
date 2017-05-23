using UnityEngine;

public class Wiggle : MonoBehaviour {
    private Animator anim;
    public Vector2 baseRotation = Vector2.zero;
    public float Speed = 4;
    private float time = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate ()
    {
        time += Time.deltaTime * Speed;
        if (anim != null)
            anim.speed = Speed * 0.2f;
        //transform.localRotation = Quaternion.Euler(baseRotation.x, Mathf.Sin(time) * 10 + baseRotation.y, 0);
    }
}
