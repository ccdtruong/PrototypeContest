using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    private GameObject Content;
    private GameObject Angry;
    private GameObject Exclamation;

    // Start is called before the first frame update
    void Start()
    {
        Content = gameObject.transform.GetChild(0).gameObject;
        Angry = gameObject.transform.GetChild(1).gameObject;
        Exclamation = gameObject.transform.GetChild(2).gameObject;
        Angry.SetActive(false);
        Exclamation.SetActive(false);
    }

    public void AngryPhase()
    {
        Content.SetActive(false);
        Angry.SetActive(true);
    }

    public void ExclamationPhase()
    {
        Angry.SetActive(false);
        Exclamation.SetActive(true);
    }
}
