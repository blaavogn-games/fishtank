using UnityEngine;

public class Wiggle : MonoBehaviour {
    public float wiggleSpeed = 4;
    private float time = 0;
    void Update () {
        time += Time.deltaTime * wiggleSpeed;
        transform.localRotation = Quaternion.Euler(0, Mathf.Sin(time) * 10, 0);
    }
}
