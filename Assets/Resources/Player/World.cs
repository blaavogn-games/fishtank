using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
    public static World i;
    [HideInInspector]
    public Vector3 SpawnPoint = Vector3.zero;

    private void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
