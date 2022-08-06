using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    [SerializeField]
    private Player m_grumpy;

    [SerializeField]
    private Player m_rebellious;

    [SerializeField]
    private Transform GrumpyMovesHere;

    [SerializeField]
    private Transform Exit;

    [SerializeField]
    private GameObject GrumpySpeech;

    [SerializeField]
    private GameObject GirlSpeech;

    [SerializeField]
    private List<GameObject> Fires;

    private bool wait = true;
    // Start is called before the first frame update
    void Start()
    {
        GrumpySpeech.SetActive(false);
        GirlSpeech.SetActive(false);
        Fires[0].SetActive(false);
        Fires[1].SetActive(false);
        Fires[2].SetActive(false);
        StartCoroutine(StartScene());
        //StartCoroutine(m_rebellious.SmallJump());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartScene()
    {
        //wait for things to initialize
        while (wait)
            {
                yield return new WaitForSeconds(0.5f);
                wait = false;
            }

        //Grumpy moves toward girl
        StartCoroutine(m_grumpy.MoveHere(GrumpyMovesHere));
        yield return new WaitForSeconds(2.0f);

        //Grumpy talks
        StartCoroutine(m_grumpy.SmallJump());
        GrumpySpeech.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        //Girl talks
        StartCoroutine(m_rebellious.SmallJump());
        GrumpySpeech.SetActive(false);
        GirlSpeech.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        //Grumpy angry
        StartCoroutine(m_grumpy.SmallJump());
        GirlSpeech.SetActive(false);
        GrumpySpeech.SetActive(true);
        GrumpySpeech.GetComponent<SpeechBubble>().AngryPhase();
        yield return new WaitForSeconds(1.0f);

        //Girl angry
        StartCoroutine(m_rebellious.SmallJump());
        GrumpySpeech.SetActive(false);
        GirlSpeech.SetActive(true);
        GirlSpeech.GetComponent<SpeechBubble>().AngryPhase();
        yield return new WaitForSeconds(1.0f);

        //Things burn
        GirlSpeech.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            Fires[i].SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }

        StartCoroutine(m_grumpy.MoveHere(Exit));
        StartCoroutine(m_rebellious.MoveHere(Exit));
    }
}
