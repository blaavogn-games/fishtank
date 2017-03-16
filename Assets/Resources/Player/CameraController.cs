using UnityEngine;

public class CameraController : MonoBehaviour {
    public int Invert = 1;
    public float LookSpeed = 10;
    [HideInInspector]
    public GameObject Player { get; set;}

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update ()
    {
        float xMove = Input.GetAxis("LookHor") * Invert;
        float yMove = Input.GetAxis("LookVer") * Invert;
        Vector3 target;
        if (xMove == 0.0f && yMove == 0.0f)
            target = transform.parent.position - transform.parent.forward * 2.5f + transform.parent.up * 1.4f;
        else {
            float magic = (Player.transform.position - transform.position).magnitude;
            magic = Mathf.Min(2, magic);
            target = transform.parent.position -
                     transform.parent.forward * 2f +
                     transform.parent.right * 3 * xMove * magic+
                     transform.parent.up * (yMove + 0.3f) * 1.4f * magic;
        }
        transform.position = Vector3.MoveTowards(transform.position, target, LookSpeed * Time.deltaTime);
        transform.LookAt(Player.transform.position + Vector3.up);
    }
}
