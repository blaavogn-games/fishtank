using UnityEngine;
using System.Collections;



public class PickupCapsule : MonoBehaviour {
    public Transform Player;
    public int rotateX = 30;
    public int rotateY = 20;
    public bool WinLevelCapsule = false;

    public ParticleSystem PickupParticleSystem;
    public ParticleSystem PickupParticleSystem2;
    public ParticleSystem PickupParticleSystem3;

    private void Start()
    {
        if (World.i.SavedPills.Contains(name))
            Destroy(gameObject);
    }

    void Update () {
        transform.Rotate(rotateX * Time.deltaTime, rotateY * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Player") return;

        if (WinLevelCapsule)
        {
            World.i.WinLevel();
            Destroy(gameObject);
            return;
        }

        if (World.i.SavedPills.Contains(name))
        {
            Destroy(gameObject);
            return;
        }

        collision.gameObject.GetComponent<PlayerSound>().Eat();       
        World.i.EatPill(name);
        Instantiate(PickupParticleSystem.gameObject, transform.position, Quaternion.identity);
        Instantiate(PickupParticleSystem2.gameObject, transform.position, Quaternion.identity);
        Instantiate(PickupParticleSystem3.gameObject, transform.position, Quaternion.identity);
        DestroyObject(gameObject);
    }
}
