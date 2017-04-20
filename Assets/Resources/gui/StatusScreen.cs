using UnityEngine;
using System.Collections;

public class StatusScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void WinScreen(int deaths, int pills, int maxPills, float timeTaken)
    {
        Debug.Log("Deaths: " + deaths + "\npills: " + pills + "/" + maxPills);
        Debug.Log("Time Taken: " + timeTaken);
    }
}
