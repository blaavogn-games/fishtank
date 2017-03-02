using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {
    [Header("Sound")]
    public AudioClip[] SwimClips;
    public AudioClip SwimAmbient;
    public AudioClip Ambient;
    public AudioClip Music;
    public AudioClip Bubbles;
    private AudioSource[] audioSources;
    private AudioLowPassFilter filter;
    
    void Start ()
    {
        audioSources = GetComponents<AudioSource>();
        audioSources[0].clip = SwimAmbient;
        audioSources[0].Play();
        audioSources[1].clip = Ambient;
        audioSources[1].Play();
        audioSources[2].clip = Music;
        audioSources[2].Play();
        audioSources[3].clip = Bubbles;
        audioSources[3].Play();
        filter = GetComponent<AudioLowPassFilter>();
        
    }

    public void SetSpeed(float speed)
    {
        //audioSources[0].volume = speed;
        filter.cutoffFrequency = 100+speed*25;
    }
}
