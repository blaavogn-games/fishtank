using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    enum State { SWIM, LOOKTRANSITION };
    private State state = State.SWIM;
    private Rigidbody _rigidbody;
    //private AudioLowPassFilter _audioLowPassFilter;
    public int InvertedAxis = 1;
    private bool dashDown = false;
    private PlayerSound playerSound;

    [Header("Swimming Controls")]
    public float MaxSwimVelocity = 15;
    public float Acceleration = 4000;
    public float HRotation = 120;
    public float VRotation = 50;
    public float DashSpeed = 30;

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



    //[Header("Snap Rotation Transition")]
    //[Tooltip("In Seconds")]
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
        playerSound = GetComponent<PlayerSound>();
    }

    void Update ()
    {
        //Debug.Log(_rigidbody.velocity.magnitude);
        switch (state)
        {
            case State.SWIM:
                float hDir = 0.0f, vDir = 0.0f, constAcc = 0.0f, boost = 0;

                //Forward movement
                //Swims if possible. Increments dash timer.
                if (Mathf.Max(Input.GetAxis("Forward"), Input.GetAxis("ForwardAlt"))> 0)
                {
                    if (MaxSwimVelocity > _rigidbody.velocity.magnitude)
                        constAcc = 1;
                    if (_rigidbody.velocity.magnitude < 7)
                        boost = 900;
                    dashTimer += Time.deltaTime;
                }
                //Dashes if timer within threshhold.
                if (Input.GetButton("Dash") && !dashDown)
                {
                    dashDown = true;
                    if (dashTimer < DashThreshold && dashCooldownTimer <= 0)
                    {
                        boost = DashSpeed;
                        dashCooldownTimer = DashCooldown;
                        swimCooldownTimer = DashCooldown * 2;
                    }
                    dashTimer = 0;
                }
                else if(!Input.GetButton("Dash"))
                {
                    dashDown = false;
                }
                float accAdded = boost + (constAcc * Acceleration * Time.deltaTime);
                _rigidbody.AddForce(accAdded * transform.forward);
                wiggle.wiggleSpeed = Mathf.Max(5,_rigidbody.velocity.magnitude + accAdded * 0.01f);

                //Turning
                vDir += InvertedAxis * Input.GetAxis("Vertical") * VRotation;
                hDir += HRotation * Input.GetAxis("Horizontal");

                transform.Rotate(new Vector3(0, hDir, 0) * Time.deltaTime, Space.World);
                transform.Rotate(new Vector3(vDir, 0, 0) * Time.deltaTime, Space.Self);
                wiggle.baseRotation.x = vDir * 0.16f;
                wiggle.baseRotation.y = hDir * 0.1f;

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
                break;
        }
        _rigidbody.velocity = transform.forward.normalized * _rigidbody.velocity.magnitude;
        playerSound.SetSpeed(_rigidbody.velocity.magnitude);
        CoolDown();

        if (Input.GetKey(KeyCode.Alpha1))
            SceneManager.LoadScene(0);
        if (Input.GetKey(KeyCode.Alpha2))
            SceneManager.LoadScene(1);
        if (Input.GetKey(KeyCode.Alpha3))
            SceneManager.LoadScene(2);
        if (Input.GetKey(KeyCode.Alpha4))
            SceneManager.LoadScene(3);
        if (Input.GetKey(KeyCode.P))
            InvertedAxis *= -1;
        if (Input.GetKey(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
    private void CoolDown()
    {
        dashCooldownTimer -= Time.deltaTime;
        if (dashCooldownTimer < 0)
            dashCooldownTimer = 0;
        swimCooldownTimer -= Time.deltaTime;
        if (swimCooldownTimer < 0)
            swimCooldownTimer = 0;
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        while(angle < -360)
            angle += 360;
        while(angle > 360)
            angle -= 360;
        return angle;
    }
}
