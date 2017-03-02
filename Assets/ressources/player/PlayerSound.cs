using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {
    [Header("Sound")]
    public AudioClip[] SwimClips;
    public AudioClip SwimAmbient;
    public AudioClip Ambient;
    private AudioSource[] audioSources;
    private AudioLowPassFilter filter;
    
    void Start ()
    {
        audioSources = GetComponents<AudioSource>();
        audioSources[0].clip = SwimAmbient;
        audioSources[0].Play();
        audioSources[1].clip = Ambient;
        audioSources[1].Play();
        filter = GetComponent<AudioLowPassFilter>();
        
    }

    public void SetSpeed(float speed)
    {
        audioSources[0].volume = speed;
        //filter.cutoffFrequency = 100+speed*25;
    }
}
