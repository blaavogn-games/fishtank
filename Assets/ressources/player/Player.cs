using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    enum State { SWIM };
    private State state = State.SWIM;
    private Rigidbody _rigidbody;
    private float _maxVelosity = 40;
    private float _acceleration = 600;
    private float _hRotation = 120;
    private float _vRotation = 50;

    void Start ()
    {
        this._rigidbody = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        switch(state)
        {
            case State.SWIM:
                int hDir = 0;
                int vDir = 0;
                //Hit cap to make movement consistent
                if(Input.GetKey(KeyCode.W) && _maxVelosity > _rigidbody.velocity.magnitude)
                    _rigidbody.AddForce(transform.forward * _acceleration * Time.deltaTime);
                if(Input.GetKey(KeyCode.D))
                    hDir += 1;
                if(Input.GetKey(KeyCode.A))
                    hDir -= 1;
                if(Input.GetKey(KeyCode.DownArrow))
                    vDir += 1;
                if(Input.GetKey(KeyCode.UpArrow))
                    vDir -= 1;
                Debug.Log("1 " + transform.rotation.y);
                transform.Rotate(new Vector3(_vRotation * vDir, _hRotation * hDir, 0) * Time.deltaTime);
                Debug.Log("2 " + transform.rotation.y);
                Debug.Log(transform.rotation.eulerAngles);

                //transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation, new Vector3(0, transform.rotation.y, 0), 2));
                break;
        }
        _rigidbody.velocity = transform.forward.normalized * _rigidbody.velocity.magnitude;
    }
}
