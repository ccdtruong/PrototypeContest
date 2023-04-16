using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    static public int s_lastLoadedScene = 0;
    private Client m_client;
    // Start is called before the first frame update

    public void Start()
    {
        if (GameSettings.m_mode == GameSettings.GameMode.Duo && SceneManager.GetActiveScene().name != "Intro")
        {
            m_client = ConnectionController.GetClient();
            if (m_client != null)
            {
                Debug.Log("LevelLoader");
                m_client.m_OnReset += Reload;
                m_client.m_OnQuit += Quit;
                m_client.m_OnReplay += LoadSpecificLevel;
            }
        }
    }

    public void OnDestroy()
    {
        if (GameSettings.m_mode == GameSettings.GameMode.Duo && m_client != null)
        {
            m_client.m_OnReset -= Reload;
            m_client.m_OnQuit -= Quit;
            m_client.m_OnReplay -= LoadSpecificLevel;
        }
    }

    public void Update()
    {
        if (m_client != null) m_client.Update();    
    }

    public void Quit()
    {
        Modal.GetInstance().ShowModal("Other player have quit the game!", "OK", LoadMainMenu);
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        //StartCoroutine(LoadLevel(0));
    }

    public void Reload()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadMainMenuAndSendMessage()
    {
        if (GameSettings.m_mode == GameSettings.GameMode.Duo)
        {
            if (m_client != null)
                m_client.Quit();
        }
        LoadMainMenu();
    }

    public void ReloadAndSendMessage()
    {
        if (GameSettings.m_mode == GameSettings.GameMode.Duo)
        {
            if (m_client != null)
            {
                m_client.Reset();
            }
        }
        Reload();
    }

    public void LoadSpecificLevel(int levelIndex)
    {
        StartCoroutine(LoadLevel(levelIndex + 2));
    }

    public void LoadSpecificLevelAndSendMessage(int levelIndex)
    {
        if (GameSettings.m_mode == GameSettings.GameMode.Duo)
        {
            if (m_client != null)
                m_client.Replay(levelIndex);
        }
        LoadSpecificLevel(levelIndex);
    }

    public void LoadSceneByName(string name)
    {
        StartCoroutine(LoadLevelByName(name));
    }

    public void LoadLastScene()
    {
        StartCoroutine(LoadLevel(s_lastLoadedScene));
    }

    public void LoadLastSceneAndSendMessage()
    {
        if (GameSettings.m_mode == GameSettings.GameMode.Duo)
        {
            if (m_client != null)
            {
                Debug.Log("Replay level " + s_lastLoadedScene);
                m_client.Replay(s_lastLoadedScene - 2);
            }
        }
        LoadLastScene();
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        LoadScene(levelIndex);
        
    }

    IEnumerator LoadLevelByName(string name)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        LoadScene(name);
    }

    private void LoadScene(string name)
    {
        s_lastLoadedScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(name);
    }

    private void LoadScene(int levelIndex)
    {
        s_lastLoadedScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex);
    }
}
