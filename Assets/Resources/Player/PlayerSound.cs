using UnityEngine;

public class PlayerSound : MonoBehaviour {
    private SoundManager soundManager;
    private AudioSource[] audioSources;

    const int AS_DASH = 0, AS_SWING = 1, AS_EAT = 2, AS_DEATH = 3, AS_COL = 4;

    void Start ()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        audioSources = GetComponents<AudioSource>();
    }

    public void Dash(float force)
    {
        audioSources[AS_DASH].volume = force / 3000;
        audioSources[AS_DASH].PlayOneShot(soundManager.GetClip(SfxTypes.DASH));
    }

    public void Swing()
    {
        audioSources[AS_SWING].PlayOneShot(soundManager.GetClip(SfxTypes.SWIM_SWING));
    }

    public void Eat()
    {
        audioSources[AS_EAT].PlayOneShot(soundManager.GetClip(SfxTypes.EAT));
    }

    public void Death()
    {
        audioSources[AS_DEATH].PlayOneShot(soundManager.GetClip(SfxTypes.DEATH));
    }

    public void CollideGround()
    {
        audioSources[AS_COL].PlayOneShot(soundManager.GetClip(SfxTypes.COL_GROUND));
    }

    public void CollideGlass()
    {
        audioSources[AS_COL].PlayOneShot(soundManager.GetClip(SfxTypes.COL_GLASS));
    }
}
