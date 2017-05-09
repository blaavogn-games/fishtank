using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {
    private SoundManager soundManager;
    private AudioSource[] audioSources;

    void Start ()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        audioSources = GetComponents<AudioSource>();
        //filter = GetComponent<AudioLowPassFilter>();
    }

    public void Dash()
    {
        audioSources[0].PlayOneShot(soundManager.GetClipGroup(SfxTypes.DASH).GetRandClip());
    }

    public void SetSpeed(float speed)
    {
        //audioSources[4].volume = speed;
        //filter.cutoffFrequency = 100+speed*25;
    }
}
