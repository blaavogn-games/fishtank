using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    enum State { SWIM, LOOKTRANSITION };
    private State state = State.SWIM;
    private Rigidbody _rigidbody;
    //private AudioLowPassFilter _audioLowPassFilter;
    public int InvertedAxis = 1;

    [Header("Swimming Controls")]
    public float MaxSwimVelocity = 15;
    public float Acceleration = 4000;
    public float HRotation = 120;
    public float VRotation = 50;

    private float dashTimer = 0;
    [Tooltip("In Seconds")]
    public float DashThreshold = 0.2f;

    private float dashCooldownTimer = 0;
    private float swimCooldownTimer = 0;
    [Tooltip("In Seconds")]
    public float DashCooldown = 0.4f;

    [Header("Mouse Look Controls")]
    public bool MouseLookEnabled = false;
    public float sensitivityX = 4F;
    public float sensitivityY = 4F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;

    [Header("Snap Rotation Transition")]
    [Tooltip("In Seconds")]
    //public float TransitionSpeed = 0.3f;
    //private Quaternion targetRotation;
    //private float transitionTimer = 0;
    //Vector3 transitionRotation = Vector3.zero;

    public Wiggle wiggle;


    void Start ()
    {
        //targetRotation = transform.rotation;
        originalRotation = transform.rotation;
        this._rigidbody = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        //GetComponent<AudioLowPassFilter>().cutoffFrequency = 100 + _rigidbody.velocity.magnitude*25;


        wiggle.wiggleSpeed = Mathf.Max(5,_rigidbody.velocity.magnitude);
        //Debug.Log(_rigidbody.velocity.magnitude);
        switch (state)
        {
            case State.SWIM:
                int hDir = 0;
                int vDir = 0;

                //Swims if possible. Increments dash timer.
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Joystick1Button0))
                {
                    if (MaxSwimVelocity > _rigidbody.velocity.magnitude && swimCooldownTimer <= 0)
                        Swim();
                    dashTimer += Time.deltaTime;
                }

                //Dashes if timer within threshhold.
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Joystick1Button0))
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
                    if (Input.GetAxis("Vertical") > 0)
                        vDir += 1 * InvertedAxis;
                    if (Input.GetAxis("Vertical") < 0)
                        vDir -= 1 * InvertedAxis;
                    if (Input.GetAxis("Horizontal") < 0)
                        hDir -= 1;
                if (Input.GetAxis("Horizontal") > 0)
                    hDir += 1;
                    
		if (Input.GetKey(KeyCode.P))
                        InvertedAxis *= -1;
                    if (Input.GetKey(KeyCode.R))
                    {
                        Scene scene = SceneManager.GetActiveScene();
                        SceneManager.LoadScene(scene.name);
                    }

                //Angle flips 90 and 180
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Joystick1Button5))
                {
                    if (MouseLookEnabled)
                        transform.rotation = originalRotation;
                    //originalRotation = transform.rotation;
                    state = State.LOOKTRANSITION;
                    //transitionTimer = 0;
                    //targetRotation = Quaternion.AngleAxis(90, Vector3.up);
                    //rotationGoal = 90;
                    LookTransition(new Vector3(0, 90, 0), Space.World);
                    originalRotation = transform.localRotation;
                }
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button4))
                {
                    if (MouseLookEnabled)
                        transform.rotation = originalRotation;
                    //originalRotation = transform.rotation;
                    state = State.LOOKTRANSITION;
                    //transitionTimer = 0;
                    //targetRotation = Quaternion.AngleAxis(-90, Vector3.up);
                    //rotationGoal = -90;
                    LookTransition(new Vector3(0, -90, 0), Space.World);
                    originalRotation = transform.localRotation;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if(MouseLookEnabled)
                        transform.rotation = originalRotation;
                    //originalRotation = transform.rotation;
                    //targetRotation = Quaternion.AngleAxis(180, Vector3.up);
                    state = State.LOOKTRANSITION;
                    //transitionTimer = 0;
                    //rotationGoal = 180;
                    LookTransition(new Vector3(0, 180, 0), Space.World);
                    originalRotation = transform.localRotation;
                }

                //Apply rotations from turning controls
                transform.Rotate(new Vector3(0, HRotation * hDir, 0) * Time.deltaTime, Space.World);
                transform.Rotate(new Vector3(VRotation * vDir, 0, 0) * Time.deltaTime, Space.Self);
                //Debug.Log(HRotation * hDir * 0.2f);
                wiggle.baseRotation.x = VRotation * vDir * 0.16f;
                wiggle.baseRotation.y = HRotation * hDir * 0.1f;

                if (MouseLookEnabled)
                {
                    rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                    rotationX = ClampAngle(rotationX, minimumX, maximumX);
                    rotationY = ClampAngle(rotationY, minimumY, maximumY);
                    Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                    Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
                    transform.rotation = originalRotation * xQuaternion * yQuaternion;
                }

                //transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation, new Vector3(0, transform.rotation.y, 0), 2));
                break;
            /*case State.LOOKTRANSITION:
                transitionTimer += Time.deltaTime/TransitionSpeed;
                //Quaternion yQuaternion2 = Quaternion.AngleAxis(rotationGoal, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, transitionTimer);
                //transform.Rotate(new Vector3(0, rotationGoal / (TransitionSpeed / Time.deltaTime), 0), Space.World);
                if (transitionTimer >= 1)
                {
                    //originalRotation = transform.rotation;
                    state = State.SWIM;
                }
                break;*/
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
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    private void LookTransition(Vector3 rotation, Space space)
    {
        transform.Rotate(rotation, space);
        state = State.SWIM;
    }
}
