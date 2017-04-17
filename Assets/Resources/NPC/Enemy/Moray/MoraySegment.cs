using UnityEngine;

// MuraneSegments are basically a double linked list 
// that allows segments to attack and retreat
public class MoraySegment : MonoBehaviour {
    public Moray Moray;
    public Transform Head, Tail;
    
    void Update ()
    {
        var target = transform.position;
        var moveDir = 0;
        switch (Moray.State)
        {
            case Moray.MorayState.ATTACK:
                moveDir = 1;
                target = Head.position;
                break;
            case Moray.MorayState.RETRACT:
                moveDir = -1;
                target = (Tail == null) ? Moray.InitialPosition : Tail.position;
                if (Vector3.Distance(transform.position, Moray.InitialPosition) < 1.0f)
                    Destroy(gameObject);
                break;
        }

        var newPosition = Vector3.MoveTowards(transform.position, target, Moray.Speed * Time.deltaTime);
        var movement = newPosition - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, movement * moveDir, 0.1f, 0));
        transform.position += transform.forward * movement.magnitude * moveDir;
    }
}
