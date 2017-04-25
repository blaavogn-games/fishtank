using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;

public enum SfxTypes
{
    DASH, EAT
};

public class AudioClipGroup
{
    public AudioClip[] Clips;
    public int Last;
    public AudioMixerGroup MixerGroup;
    public AudioClipGroup(AudioClip[] clips, AudioMixerGroup mixerGroup)
    {
        Clips = clips;
        Last = -1;
        mixerGroup = MixerGroup;
    }

    public AudioClip GetRandClip()
    {
        if (Clips.Length == 1)
            return Clips[0];
        var i = UnityEngine.Random.Range(0, Clips.Length - 1);
        if (i == Last)
            i++;
        Last = i;
        return Clips[i];
    }
}

public class SoundManager : MonoBehaviour {
    private static Dictionary<SfxTypes, AudioClipGroup> map;
    //Has to be set through editor
    public AudioMixerGroup Music, SFX, LeadSFX, Ambient;

    void Start () {
        map = new Dictionary<SfxTypes, AudioClipGroup>();
        map.Add(SfxTypes.DASH, new AudioClipGroup(Resources.LoadAll<AudioClip>("SFX/dash"), SFX));
        map.Add(SfxTypes.EAT, new AudioClipGroup(Resources.LoadAll<AudioClip>("SFX/eat"), SFX));
    }
	
    public AudioClipGroup GetRandClip(SfxTypes type)
    {
        return map[type];
    }
}
