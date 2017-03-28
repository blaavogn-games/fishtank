using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Fog : MonoBehaviour {
    public float FogIncreasePerSecond = 0.008F;
    public float MaxFogLevel = 0.008f;
    public Color FogColor;
    void Start () {
        RenderSettings.fogDensity = 0.0F;
        RenderSettings.fogColor = FogColor;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fog = true;
        RenderSettings.fogDensity = MaxFogLevel;
    }

    void Update () {
        if(Input.GetKey(KeyCode.Z) && RenderSettings.fogDensity > 0.006)
            RenderSettings.fogDensity = RenderSettings.fogDensity - Time.deltaTime * 0.08f;
        else if(RenderSettings.fogDensity < MaxFogLevel)
            RenderSettings.fogDensity = RenderSettings.fogDensity + Time.deltaTime * 0.08f;
    }
}
