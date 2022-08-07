using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        //SoundManager.Instance.Play("background");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void PlayClickSound()
    {
        SoundManager.Instance.Play("click");
    }
    public void UpdateSliderValue(float val)
    {

    }
}
