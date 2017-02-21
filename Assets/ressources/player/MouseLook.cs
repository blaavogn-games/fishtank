using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {
    public float sensitivityX = 4F;
    public float sensitivityY = 4F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    private float rotX = 0;
    private float rotY = 0;
    Quaternion originalRotation;
    void Update()
    {
        // Read the mouse input axis
        /*Debug.Log(Input.GetAxis("Mouse X"));
        Debug.Log(Input.GetAxis("Mouse Y"));

        if(Input.GetAxis("Mouse X") > 0)
            rotX = 1;
        if (Input.GetAxis("Mouse X") < 0)
            rotX = -1;
        if (Input.GetAxis("Mouse Y") > 0)
            rotY = -1;
        if (Input.GetAxis("Mouse Y") < 0)
            rotY = 1;
        
        transform.Rotate(new Vector3(rotY*sensitivityY, rotX*sensitivityX, 0));*/

        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
        transform.rotation = originalRotation * xQuaternion * yQuaternion;
        rotX = 0;
        rotY = 0;
    }
    void Start()
    {
        // Make the rigid body not change rotation
        originalRotation = transform.localRotation;
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
         angle += 360F;
        if (angle > 360F)
         angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
