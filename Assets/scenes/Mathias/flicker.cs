using UnityEngine;
using System.Collections;

public class flicker : MonoBehaviour
{

    public float timeOnMin = 0.1f;
    public float timeOnMax = 0.1f;
    public float timeOffMin = 0.5f;
    public float timeOffMax = 0.5f;
    private float changeTime = 0f;



    // Update is called once per frame
    void Update()
    {

        if (Time.time > changeTime)
        {
            GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
            if (GetComponent<Light>().enabled)
            {
                changeTime = Time.time + Random.Range(timeOnMin,timeOnMax);
            }
            else
            {
                changeTime = Time.time + Random.Range(timeOffMin, timeOffMax);
            }
        }
    }
}