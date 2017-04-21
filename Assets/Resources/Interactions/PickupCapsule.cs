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
        if (World.i.savedPills.Contains(name))
            Destroy(gameObject);
    }

    void Update () {
        transform.Rotate(rotateX * Time.deltaTime, rotateY * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var script = collision.gameObject.GetComponent<Player>();
        if (script != null)
        {
            if (WinLevelCapsule)
            {
                World.i.WinLevel();
                Destroy(gameObject);
                return;
            }
       
            if (World.i.savedPills.Contains(name))
                return;
            Trigger trig = GetComponentInChildren<Trigger>();
            World.i.Pill(name);
            DestroyObject(gameObject);
            if(script.MaxHunger>0)
                script.hunger = script.MaxHunger;
            var p = Instantiate(PickupParticleSystem.gameObject);
            p.transform.position = transform.position;
            var p2 = Instantiate(PickupParticleSystem2.gameObject);
            p2.transform.position = transform.position;
            var p3 = Instantiate(PickupParticleSystem3.gameObject);
            p3.transform.position = transform.position;
            if (trig != null)
            {
                trig.TriggerByParent(collision);
            }
        }
    }
}
