using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;

public enum SfxTypes
{
    DASH, EAT, SWIM_SWING, DEATH, MORRAY_ATTACK, COL_GROUND, COL_GLASS, FF_PICKUP
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

    void Start() {
        map = new Dictionary<SfxTypes, AudioClipGroup>();
        map.Add(SfxTypes.DASH, new AudioClipGroup(Resources.LoadAll<AudioClip>("SFX/dash"), SFX));
        map.Add(SfxTypes.EAT, new AudioClipGroup(Resources.LoadAll<AudioClip>("SFX/eatPill"), SFX));
        map.Add(SfxTypes.SWIM_SWING, new AudioClipGroup(Resources.LoadAll<AudioClip>("SFX/swim"), SFX));
        map.Add(SfxTypes.DEATH, new AudioClipGroup(Resources.LoadAll<AudioClip>("SFX/death"), SFX)); 
        map.Add(SfxTypes.MORRAY_ATTACK, new AudioClipGroup(Resources.LoadAll<AudioClip>("SFX/moray"), SFX)); 
        map.Add(SfxTypes.COL_GLASS, new AudioClipGroup(Resources.LoadAll<AudioClip>("SFX/col_glass"), SFX)); 
        map.Add(SfxTypes.COL_GROUND, new AudioClipGroup(Resources.LoadAll<AudioClip>("SFX/col_ground"), SFX));
        map.Add(SfxTypes.FF_PICKUP, new AudioClipGroup(Resources.LoadAll<AudioClip>("SFX/pickup_FF"), SFX));
    }
	
    public AudioClip GetClip(SfxTypes type)
    {
        return map[type].GetRandClip();
    }

    public void PlayEndMusic()
    {
        
    }
}
