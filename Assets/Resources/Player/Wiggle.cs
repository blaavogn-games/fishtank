using UnityEngine;

public class Wiggle : MonoBehaviour {
    public Vector2 baseRotation = Vector2.zero;
    public float wiggleSpeed = 4;
    private float time = 0;

    void Update () {
        time += Time.deltaTime * wiggleSpeed;
        transform.localRotation = Quaternion.Euler(baseRotation.x, Mathf.Sin(time) * 10 + baseRotation.y, 0);
    }
}
