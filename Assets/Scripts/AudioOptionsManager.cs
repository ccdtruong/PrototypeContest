using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOptionsManager : MonoBehaviour
{
    public static float musicVol { get; private set; }
    public static float sfxVol { get; private set; }
    public void OnMusicSlideValueChange(float val)
    {
        musicVol = val;
        SoundManager.Instance.UpdateMixerVol();
    }
    public void OnSFXSlideValueChange(float val)
    {
        SoundManager.Instance.UpdateMixerVol();
        sfxVol = val;
    }
}
