using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour {
    public float FogIncreasePerSecond = 0.01F;
    public float MaxFogLevel = 0.025f;
    public Color FogColor;
	// Use this for initialization
	void Start () {
        RenderSettings.fogDensity = 0.0F;
        RenderSettings.fogColor = FogColor;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fog = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (RenderSettings.fogDensity <= MaxFogLevel)
            RenderSettings.fogDensity += FogIncreasePerSecond * Time.deltaTime;
        else
            RenderSettings.fogDensity = MaxFogLevel;
    }
}
