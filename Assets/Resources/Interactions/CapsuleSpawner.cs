using UnityEngine;
using System.Collections;

public class CapsuleSpawner : MonoBehaviour {
    public float SpawnInterval = 1;
    private float spawnTimer = 0;
    public GameObject Capsule;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (spawnTimer > SpawnInterval)
        {
            var p=Instantiate(Capsule);
            p.transform.position = new Vector3(Random.Range(-80, 80), Random.Range(60, 70), Random.Range(5, 75));
            p.transform.Rotate(new Vector3(Random.Range(1, 5), Random.Range(1, 5), Random.Range(1, 5)));
            spawnTimer = 0;
        }
        spawnTimer += Time.deltaTime;
	}
}
