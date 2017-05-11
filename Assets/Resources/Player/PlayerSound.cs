using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {
    private SoundManager soundManager;
    private AudioSource[] audioSources;

    const int AS_DASH = 0, AS_SWING = 1, AS_EAT = 2;

    void Start ()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        audioSources = GetComponents<AudioSource>();
    }

    public void Dash(float force)
    {
        audioSources[AS_DASH].volume = force / 3000;
        audioSources[AS_DASH].PlayOneShot(soundManager.GetClipGroup(SfxTypes.DASH).GetRandClip());
    }

    public void Swing()
    {
        audioSources[AS_SWING].PlayOneShot(soundManager.GetClipGroup(SfxTypes.SWIM_SWING).GetRandClip());
    }

    public void Eat()
    {
        audioSources[AS_EAT].PlayOneShot(soundManager.GetClipGroup(SfxTypes.EAT).GetRandClip());
    }
}
