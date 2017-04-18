using UnityEngine;
using System.Collections;

public class Wavy : MonoBehaviour {
    float xOffset = 0, yOffset = 0;
    Vector3 initialRotation;
    void Start(){
        xOffset = transform.position.x * 0.2f; //Might be more effecient to calculate this as an initial roation offset
        yOffset = transform.position.y * 0.2f + 1.5f;
        initialRotation = transform.rotation.eulerAngles;
    }

    void Update () {
        transform.rotation = Quaternion.Euler(Mathf.Sin((Time.time + xOffset) * 1.2f) * 6 + initialRotation.x, 
                                              Mathf.Sin((Time.time + yOffset) * 1.2f) * 5 + initialRotation.y,
                                              initialRotation.z);
    }
}
