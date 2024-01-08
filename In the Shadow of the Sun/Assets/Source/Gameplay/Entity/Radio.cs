using System;
using Unity.VisualScripting;
using UnityEngine;

public enum EMusicType
{
    Jazz,
    Win,
    Lose
}

public class Radio : MonoBehaviour
{
    private static Radio instance;
    public static Radio Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Radio>();
            }

            return instance;
        }
    }
    
    public AudioSource source;
    public AudioClip jazzTheme;
    public AudioClip gameWinTheme;
    public AudioClip gameLoseTheme;

    private void Start()
    {
        instance = this;
    }

    public void Play(EMusicType music)
    {
        switch (music)
        {
            case EMusicType.Jazz:
                source.clip = jazzTheme;
                break;
            case EMusicType.Lose:
                source.clip = gameLoseTheme;
                break;
            case EMusicType.Win:
                source.clip = gameWinTheme;
                break;
        }
        
        source.Play();
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }
}
