using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    enum State { SWIM };
    private State state = State.SWIM;
    private Rigidbody _rigidbody;
    private float _maxVelosity = 20;
    private float _acceleration = 4000;
    private float _hRotation = 120;
    private float _vRotation = 50;
    public Wiggle wiggle;

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
                if(Input.GetKey(KeyCode.W) && _maxVelosity > _rigidbody.velocity.magnitude)
                    _rigidbody.AddForce(transform.forward * _acceleration * Time.deltaTime);
                if(Input.GetKey(KeyCode.UpArrow))
                    vDir -= 1;
                if(Input.GetKey(KeyCode.DownArrow))
                    vDir += 1;
                if(Input.GetAxis("Horizontal") > 0)
                    hDir += 1;
                if(Input.GetAxis("Horizontal") < 0)
                    hDir -= 1;
                transform.Rotate(new Vector3(0, _hRotation * hDir, 0) * Time.deltaTime, Space.World);
                transform.Rotate(new Vector3(_vRotation * vDir, 0, 0) * Time.deltaTime, Space.Self);
                Debug.Log(_hRotation * hDir * 0.2f);
                wiggle.baseRotation.x = _vRotation * vDir * 0.16f;
                wiggle.baseRotation.y = _hRotation * hDir * 0.1f;

                //transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation, new Vector3(0, transform.rotation.y, 0), 2));
                break;
        }
        _rigidbody.velocity = transform.forward.normalized * _rigidbody.velocity.magnitude;
    }
}
