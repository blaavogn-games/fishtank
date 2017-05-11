using UnityEngine;
using System.Collections;

//Cancer
public class FollowFishSound : MonoBehaviour {
    SoundManager soundManager;
    AudioSource audioSource;
	void Start () {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Swim()
    {
        audioSource.PlayOneShot(soundManager.GetClip(SfxTypes.SWIM_SWING));
    }
}
