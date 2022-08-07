using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
public enum SoundType
{
    SFX,
    Music
}
[System.Serializable]
public class Sound
{
    public SoundType type;
    [HideInInspector] public AudioSource source;
    public string clipName;
    public AudioClip audioClip;
    public bool isLoop;
    public bool playOnAwake;
    [Range(0, 1)]
    public float volume = 0.5f;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private Sound[] sounds;
    private void Awake()
    {
        Instance = this;
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip;
            s.source.loop = s.isLoop;
            s.source.volume = s.volume;

            switch (s.type)
            {
                case SoundType.Music:
                    s.source.outputAudioMixerGroup = musicGroup;
                    break;
                case SoundType.SFX:
                    s.source.outputAudioMixerGroup = sfxGroup;
                    break;
            }
            if (s.playOnAwake)
                s.source.Play();
        }
    }
    public void Play(string clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if (s == null)
        {
            Debug.LogError("Sound:" + clipname + "does NOT exist!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string clipname)
    {
        Sound s = Array.Find(sounds, dummySound => dummySound.clipName == clipname);
        if (s == null)
        {
            Debug.LogError("Sound:" + clipname + "does NOT exist!");
            return;
        }
        s.source.Stop();
    }
    public void UpdateMixerVol()
    {
        musicGroup.audioMixer.SetFloat("musicVol", Mathf.Log10(AudioOptionsManager.musicVol) * 20);
        musicGroup.audioMixer.SetFloat("sfxVol", Mathf.Log10(AudioOptionsManager.sfxVol) * 20);
    }
    public void PlayClickSound()
    {
        SoundManager.Instance.Play("click");
    }
}
