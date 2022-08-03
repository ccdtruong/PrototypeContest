using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip collideSound;
    public static AudioClip backgroundMusic;
    public static AudioClip tingSound;

    private static AudioSource m_audioSource;

    private void Start()
    {
        collideSound = Resources.Load<AudioClip>("Sounds/collide");
        backgroundMusic = Resources.Load<AudioClip>("Sounds/background");
        tingSound = Resources.Load<AudioClip>("Sounds/ting");
        m_audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip_name)
    {
        switch(clip_name)
        {
            case "collide":
                {
                    m_audioSource.PlayOneShot(collideSound);
                    break;
                }
            case "background":
                {
                    m_audioSource.clip = backgroundMusic;
                    m_audioSource.loop = true;
                    m_audioSource.Play();
                    break;
                }
            case "ting":
                {
                    m_audioSource.PlayOneShot(tingSound);
                    break;
                }
        }
    }

    public static void StopSound()
    {
        m_audioSource.Stop();
    }

    public static bool IsPlaying(string clip_name)
    {
            return m_audioSource.isPlaying;
    }
}
