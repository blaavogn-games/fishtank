using UnityEngine;

public class Camera : MonoBehaviour {
    public GameObject Target;
    public float DefuaultFishDist = 5;
    private float targetFishDist, heightRatio = .5f;
    private Vector3 rotation = new Vector3(20,0,0); //Should probably be computed from Ratio or vice versa

    void Start ()
    {
        targetFishDist = DefuaultFishDist;
    }

    void Update ()
    {
        Quaternion playerRotation = Target.transform.rotation;
        transform.rotation = Target.transform.rotation;
        transform.Rotate(rotation);
        float deltaH = targetFishDist * heightRatio;
        float deltaD = targetFishDist;
        transform.position = Target.transform.position + Target.transform.forward * -1 * deltaD  + Target.transform.up * deltaH;
    }
}
