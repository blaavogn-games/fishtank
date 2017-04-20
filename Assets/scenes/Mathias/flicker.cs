using UnityEngine;
using System.Collections;

public class flicker : MonoBehaviour
{

    public float timeOnMin = 0.2f;
    public float timeOnMax = 0.5f;
    public float timeOffMin = 0.01f;
    public float timeOffMax = 0.1f;
    private float changeTime = 0f;
    Light[] lights = new Light[3];


    // Update is called once per frame
    void Start()
    {
        for (int i = 0; i<transform.childCount; i++)
        {
            lights[i] = transform.GetChild(i).GetComponent<Light>();
        }
    }
    void Update()
    {

        if (Time.time > changeTime)
        {
            foreach (Light l in lights)
            {
                l.enabled = !l.enabled;

                if (l.enabled)
                {
                    changeTime = Time.time + Random.Range(timeOnMin, timeOnMax);
                }
                else
                {
                    changeTime = Time.time + Random.Range(timeOffMin, timeOffMax);
                }
            }
            /*GetComponentInChildren<Light>().enabled = !GetComponentInChildren<Light>().enabled;
            if (GetComponentInChildren<Light>().enabled)
            {
                changeTime = Time.time + Random.Range(timeOnMin,timeOnMax);
            }
            else
            {
                changeTime = Time.time + Random.Range(timeOffMin, timeOffMax);
            }*/
        }
    }
}