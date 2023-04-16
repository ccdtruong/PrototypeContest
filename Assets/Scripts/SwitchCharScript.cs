using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCharScript : MonoBehaviour
{
    [SerializeField]
    GameObject m_btnPlay;
    [SerializeField]
    GameObject m_btnBack;
    [SerializeField]
    GameObject m_btnSwitch;
    [SerializeField]
    Image m_imgChar;
    [SerializeField]
    LevelLoader m_levelLoader;

    void OnEnable()
    {
        var client = ConnectionController.GetClient();
        if (ConnectionController.GetClient() is Host)
        {
            Debug.Log("is Host");
            m_btnPlay.SetActive(true);
            m_btnSwitch.SetActive(true);
            GameSettings.SwitchChar(GameSettings.Character.Grumpy);
            m_imgChar.sprite = Resources.Load<Sprite>("Players/Grumpy/character_malePerson_idle");
        } else
        {
            Debug.Log("is Participant");
            m_btnPlay.SetActive(false);
            m_btnSwitch.SetActive(false);
            ((Participant)client).m_OnPlay += OnPlay;
            ((Participant)client).m_OnSwitchCharacter += OnSwitchCharacter;
            GameSettings.SwitchChar(GameSettings.Character.Rebellious);
            m_imgChar.sprite = Resources.Load<Sprite>("Players/Rebellious/character_femaleAdventurer_idle");
        }
    }

    void Update()
    {
        ConnectionController.Update();
    }

    public void SwitchCharacterBtnClick()
    {
        var client = (Host)ConnectionController.GetClient();
        client.SwitchCharacter();
        SwitchCharacter();
    }

    public void SwitchCharacter()
    {
        if (GameSettings.m_chosenChar == GameSettings.Character.Grumpy)
        {
            GameSettings.SwitchChar(GameSettings.Character.Rebellious);
            m_imgChar.sprite = Resources.Load<Sprite>("Players/Rebellious/character_femaleAdventurer_idle");
        }
        else
        {
            GameSettings.SwitchChar(GameSettings.Character.Grumpy);
            m_imgChar.sprite = Resources.Load<Sprite>("Players/Grumpy/character_malePerson_idle");
        }
    }

    public void StartGame()
    {
        GameSettings.SetGameMode(GameSettings.GameMode.Duo);
        m_levelLoader.LoadNextLevel();
    }

    public void Play()
    {
        var client = (Host)ConnectionController.GetClient();
        client.Play();
        StartGame();
    }

    public void OnBack()
    {
        ConnectionController.Close();
    }

    void OnSwitchCharacter()
    {
        Debug.Log("Switch Char");
        SwitchCharacter();
    }

    void OnPlay()
    {
        Debug.Log("Play");
        StartGame();
    }
}
