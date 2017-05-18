using UnityEngine;
using System.Collections;

public class FollowFish : MonoBehaviour {
    private enum State { FOLLOWING, FOLLOWIDLE, IDLE, FLEE }
    private State state = State.IDLE;
    private PlayerFollowers player;
    private int followId;
    public float MinSpeed = 10, MaxSpeed = 30, IdleSpeed = 3, FleeSpeed = 10;
    private float[] speedBuffer = new float[15];
    private int curSpeed = 0, playerSpeed;
    private float lastFleeTime = 0;
    public Wiggle wiggle;
    private Vector3 idleTarget, initialPosition;
    private Vector3 lastPos, lastTarget;
    public ParticleSystem ParticleSystem;
    
    public bool WillFollowPlayer = true;
    public GameObject DeathEffect;
    private AudioSource audioSource;
    private SoundManager soundManager;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        playerSpeed = speedBuffer.Length - 2;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFollowers>();
        idleTarget = RandomTarget(transform.position);
        initialPosition = transform.position;
        lastPos = transform.position;
    }

    void FixedUpdate() {
        switch (state) {
            case State.FOLLOWING:
                speedBuffer[(playerSpeed++) % speedBuffer.Length] = player.GetComponent<Rigidbody>().velocity.magnitude;
                float speed = Mathf.Max(MinSpeed, speedBuffer[(curSpeed++) % speedBuffer.Length]) * 1.2f;
                speed = Mathf.Min(speed, MaxSpeed);
                Vector3 target = ChaseTarget(player.transform, followId);
                if (Move(target, speed) && Vector3.Distance(target, lastTarget) < 0.02f)
                {
                    state = State.FOLLOWIDLE;
                    idleTarget = RandomTarget(player.transform.position);
                }
                lastTarget = target;
                break;
            case State.FOLLOWIDLE:
                if (Move(idleTarget, IdleSpeed))
                    idleTarget = RandomTarget(player.transform.position);
                if (Vector3.Distance(transform.position, player.transform.position) > 3)
                    state = State.FOLLOWING;
                break;
            case State.IDLE:
                if (Move(idleTarget, IdleSpeed))
                    idleTarget = RandomTarget(initialPosition);
                if (WillFollowPlayer)
                {
                    if (Vector3.Distance(transform.position, player.transform.position) < 5)
                    {
                        audioSource.PlayOneShot(soundManager.GetClip(SfxTypes.FF_PICKUP));
                        followId = player.AddFollower(gameObject);
                        state = State.FOLLOWING;
                        if (ParticleSystem != null) {
                            ParticleSystem.SetParticles(null, 0);
                            ParticleSystem.Stop();
                        }
                    }
                }
                else if (Vector3.Distance(transform.position, player.transform.position) < 8 && Time.time - lastFleeTime > 2)
                {
                    state = State.FLEE;
                }
                break;
            case State.FLEE:
                lastFleeTime = Time.time;
                Move(transform.position + (transform.position - player.transform.position), FleeSpeed);
                if (Vector3.Distance(transform.position, player.transform.position) > 14)
                {
                    idleTarget = initialPosition;
                    state = State.IDLE;
                }
                break;
        }
        wiggle.Speed = Vector3.Distance(lastPos, transform.position) * 50;
        lastPos = transform.position;
    }

    private Vector3 ChaseTarget(Transform t, int id)
    {
        float forward = 0, right = 0, up = 0;
        switch(id) //Could be more effecient
        {
            case 0:
                forward = 1.5f;
                right = -2f;
                up = 0.1f;
                break;
            case 1:
                forward = 1.6f;
                right = 0.3f;
                up = 2.1f;
                break;
            case 2:
                forward = 1.7f;
                right = 1.3f;
                up = -0.2f;
                break;
            case 3:
                forward = 1.3f;
                right = 1.1f;
                up = -0.4f;
                break;
        }

        return t.position + t.forward * forward + t.right * right + t.up * up;
    }

    public void Respawn()
    {
        Instantiate(DeathEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(soundManager.GetClip(SfxTypes.DEATH));
        state = State.IDLE;
        if (ParticleSystem != null)
            ParticleSystem.Play();
        player.RemoveFollower(gameObject); //Perhaps followers should just be infered from whether they are IDLE or not
        transform.position = initialPosition;
        idleTarget = RandomTarget(initialPosition);
    }

    private Vector3 RandomTarget(Vector3 center)
    {
        Vector3 newPos;
        do
            newPos = center + new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        while (Vector3.Distance(newPos, transform.position) < 1.0f);
        return newPos;
    }

    private bool Move(Vector3 target, float speed)
    {
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 movement = newPosition - transform.position;
        wiggle.Speed = movement.magnitude * 10;
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, movement, 0.1f, 0));
        transform.position += transform.forward * movement.magnitude;
        return (Vector3.Distance(transform.position, target) < 0.5f);
    }
}
