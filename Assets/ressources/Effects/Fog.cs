﻿using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour {
    public float FogIncreasePerSecond = 0.01F;
    public float MaxFogLevel = 0.04f;
	// Use this for initialization
	void Start () {
        RenderSettings.fogDensity = 0.0F;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fog = true;
    }
	
	// Update is called once per frame
	void Update () {
        RenderSettings.fogDensity += FogIncreasePerSecond * Time.deltaTime;
    }
}
