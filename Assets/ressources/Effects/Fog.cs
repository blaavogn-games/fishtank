using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour {
    public float FogIncreasePerSecond = 0.008F;
    public float MaxFogLevel = 0.008f;
    public Color FogColor;
    void Start () {
        RenderSettings.fogDensity = 0.0F;
        RenderSettings.fogColor = FogColor;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fog = true;
    }

    void Update () {
            RenderSettings.fogDensity += FogIncreasePerSecond * Time.deltaTime;
        if(RenderSettings.fogDensity>MaxFogLevel)
            RenderSettings.fogDensity = MaxFogLevel;

        if (RenderSettings.fogColor != FogColor)
            RenderSettings.fogColor = FogColor;
    }
}
