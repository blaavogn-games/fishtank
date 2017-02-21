using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    enum State { SWIM };
    private State state = State.SWIM;
    private Rigidbody _rigidbody;
    public float MaxSwimVelocity = 15;
    public float DashVelocity = 25;
    public float Acceleration = 4000;
    public float Decceleration = 1000;
    public float HRotation = 120;
    public float VRotation = 50;
    public Wiggle wiggle;

    private float dashTimer = 0;
    [Tooltip("In Seconds")]
    public float DashThreshold = 0.5f;

    private float dashCooldownTimer = 0;
    [Tooltip("In Seconds")]
    public float DashCooldown = 0.34f;

    void Start ()
    {
        this._rigidbody = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        wiggle.wiggleSpeed = Mathf.Max(5,_rigidbody.velocity.magnitude);
        Debug.Log(_rigidbody.velocity.magnitude);
        switch (state)
        {
            case State.SWIM:
                int hDir = 0;
                int vDir = 0;
                //Hit cap to make movement consistent
                if (Input.GetKey(KeyCode.W))
                {
                        if (MaxSwimVelocity > _rigidbody.velocity.magnitude && dashCooldownTimer*2<=0)
                            Swim();
                    dashTimer += Time.deltaTime;
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    dashTimer = 0;
                }
                if (Input.GetKeyUp(KeyCode.W))
                {
                    if(dashTimer<DashThreshold && dashCooldownTimer <= 0)
                    {
                        Dash();
                        dashCooldownTimer = DashCooldown;
                    }
                }
                if(Input.GetKey(KeyCode.UpArrow))
                    vDir += 1;
                if(Input.GetKey(KeyCode.DownArrow))
                    vDir -= 1;
                if (Input.GetKey(KeyCode.LeftArrow))
                    hDir -= 1;
                if (Input.GetKey(KeyCode.RightArrow))
                    hDir += 1;

                if (Input.GetKeyDown(KeyCode.D))
                {
                    transform.Rotate(new Vector3(0, 90, 0), Space.World);
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    transform.Rotate(new Vector3(0, -90, 0), Space.World);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    transform.Rotate(new Vector3(0, 180, 0), Space.World);
                }
                //Debug.Log(transform.rotation.eulerAngles);
                transform.Rotate(new Vector3(0, HRotation * hDir, 0) * Time.deltaTime, Space.World);
                transform.Rotate(new Vector3(VRotation * vDir, 0, 0) * Time.deltaTime, Space.Self);
                Debug.Log(HRotation * hDir * 0.2f);
                wiggle.baseRotation.x = VRotation * vDir * 0.16f;
                wiggle.baseRotation.y = HRotation * hDir * 0.1f;

                //transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation, new Vector3(0, transform.rotation.y, 0), 2));
                break;
        }
        _rigidbody.velocity = transform.forward.normalized * _rigidbody.velocity.magnitude;
        //Count down dash cooldown
        dashCooldownTimer -= Time.deltaTime;
        if (dashCooldownTimer < 0)
            dashCooldownTimer = 0;
    }
    private void Dash()
    {
        _rigidbody.AddForce(transform.forward * Acceleration*50 * Time.deltaTime);
    }
    private void Swim()
    {
        _rigidbody.AddForce(transform.forward * Acceleration * Time.deltaTime);
    }
}
