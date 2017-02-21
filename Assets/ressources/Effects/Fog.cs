using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour {
    public float fogIncreasePerSecond = 0.01F;
	// Use this for initialization
	void Start () {
        RenderSettings.fogDensity = 0.0F;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fog = true;
    }
	
	// Update is called once per frame
	void Update () {
        RenderSettings.fogDensity += fogIncreasePerSecond * Time.deltaTime;
    }
}
