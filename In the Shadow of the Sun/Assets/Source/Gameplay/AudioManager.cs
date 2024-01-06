using System;
using UnityEngine;

public enum ESoundEffect : int
{
    MoneyChing = 0,
    PaperSlap = 1,
    PaperOpen1 = 2,
    PaperOpen2 = 3,
    StaffHire = 4,
    StaffFire = 5,
    PhoneDial1 = 6,
    PhoneDial2 = 7,
    PhoneDial3 = 8,
    Vinyl = 9
}

[Serializable]
public class AudioSourceReference
{
    public ESoundEffect effect;
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public AudioSourceReference[] audioSources;

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }

            return instance;
        }
    }

    private void OnEnable()
    {
        instance = this;
    }

    public void PlayEffect(ESoundEffect effect)
    {
        audioSources[effect.GetHashCode()].source.Play();
    }
}
