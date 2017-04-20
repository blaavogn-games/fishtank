using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {
    Transform pivot;
    //in seconds
    float shake = 0;
    float shakeIntensity = 0.7f;
    float shakeY = 0;
    float shakeX = 0;
    float shakeFalloff = 0.7f;
    float shakeTime = 0;
    float falloff = 0;

	// Use this for initialization
	void Start () {
        pivot = transform.parent;

	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (shake > 0)
        {
            shakeY = Random.Range(-shakeIntensity, shakeIntensity);
            shakeX = Random.Range(-shakeIntensity, shakeIntensity);
            pivot.transform.localPosition = new Vector3(pivot.transform.localPosition.x+shakeX, pivot.transform.localPosition.y+shakeY, pivot.transform.localPosition.z);
            if(falloff<=0)
                shakeIntensity = Mathf.Lerp(0, shakeFalloff, shake/shakeTime);
        }
        falloff -= Time.deltaTime;
        shake -= Time.deltaTime;
        if (shake < 0)
            shake = 0;
        if (falloff < 0)
            falloff = 0;
	}
    public void ShakeScreen(float time, float intensity)
    {
        shake = time;
        shakeTime = time;
        shakeIntensity = intensity;
        shakeFalloff = intensity;
    }
    public void ShakeScreen(float time, float intensity, float falloffPoint)
    {
        shake = time;
        shakeTime = time;
        shakeIntensity = intensity;
        shakeFalloff = intensity;
        falloff = falloffPoint;
    }
}
