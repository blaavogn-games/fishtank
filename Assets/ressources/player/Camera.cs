using UnityEngine;

public class Camera : MonoBehaviour {
    public GameObject Target;
    private float _defuaultFishDist = 5, _targetFishDist, _heightRatio = .5f;
    private Vector3 _rotation = new Vector3(20,0,0); //Should probably be computed from Ratio or vice versa

    void Start ()
    {
        _targetFishDist = _defuaultFishDist;
    }

    void Update ()
    {
        Quaternion playerRotation = Target.transform.rotation;
        transform.rotation = Target.transform.rotation;
        transform.Rotate(_rotation);
        float deltaH = _targetFishDist * _heightRatio;
        float deltaD = _targetFishDist;
        transform.position = Target.transform.position + Target.transform.forward * -1 * deltaD  + Target.transform.up * deltaH;
    }
}
