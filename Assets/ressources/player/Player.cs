using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    enum State { SWIM };
    private State state = State.SWIM;
    private Rigidbody _rigidbody;
    public float MaxSwimVelocity = 15;
    public float Acceleration = 4000;
    public float HRotation = 120;
    public float VRotation = 50;
    public Wiggle wiggle;

    private float dashTimer = 0;
    [Tooltip("In Seconds")]
    public float DashThreshold = 0.2f;

    private float dashCooldownTimer = 0;
    private float swimCooldownTimer = 0;
    [Tooltip("In Seconds")]
    public float DashCooldown = 0.4f;

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

                //Swims if possible. Increments dash timer.
                if (Input.GetKey(KeyCode.W))
                {
                    if (MaxSwimVelocity > _rigidbody.velocity.magnitude && swimCooldownTimer <= 0)
                        Swim();
                    dashTimer += Time.deltaTime;
                }

                //Dashes if timer within threshhold.
                if (Input.GetKeyUp(KeyCode.W))
                {
                    if (dashTimer < DashThreshold && dashCooldownTimer <= 0)
                    {
                        Dash();
                        dashCooldownTimer = DashCooldown;
                        swimCooldownTimer = DashCooldown * 2;
                    }
                    dashTimer = 0;
                }

                //Turning
                if(Input.GetKey(KeyCode.UpArrow))
                    vDir += 1;
                if(Input.GetKey(KeyCode.DownArrow))
                    vDir -= 1;
                if (Input.GetKey(KeyCode.LeftArrow))
                    hDir -= 1;
                if (Input.GetKey(KeyCode.RightArrow))
                    hDir += 1;

                //Angle flips 90 and 180
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

                //Apply rotations from turning controls
                transform.Rotate(new Vector3(0, HRotation * hDir, 0) * Time.deltaTime, Space.World);
                transform.Rotate(new Vector3(VRotation * vDir, 0, 0) * Time.deltaTime, Space.Self);
                Debug.Log(HRotation * hDir * 0.2f);
                wiggle.baseRotation.x = VRotation * vDir * 0.16f;
                wiggle.baseRotation.y = HRotation * hDir * 0.1f;

                //transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation, new Vector3(0, transform.rotation.y, 0), 2));
                break;
        }
        _rigidbody.velocity = transform.forward.normalized * _rigidbody.velocity.magnitude;

        CoolDown();
    }
    private void Dash()
    {
        _rigidbody.AddForce(transform.forward * Acceleration*50 * Time.deltaTime);
    }
    private void Swim()
    {
        _rigidbody.AddForce(transform.forward * Acceleration * Time.deltaTime);
    }
    private void CoolDown()
    {
        //Count down dash cooldown
        dashCooldownTimer -= Time.deltaTime;
        if (dashCooldownTimer < 0)
            dashCooldownTimer = 0;

        //Count down sim cooldown
        swimCooldownTimer -= Time.deltaTime;
        if (swimCooldownTimer < 0)
            swimCooldownTimer = 0;
    }
}
