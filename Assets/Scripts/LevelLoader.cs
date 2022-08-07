using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    static public int s_lastLoadedScene = 0;
    // Start is called before the first frame update
    
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

    public void LoadSpecificLevel(int levelIndex)
    {
        StartCoroutine(LoadLevel(levelIndex + 2));
    }

    public void LoadSceneByName(string name)
    {
        StartCoroutine(LoadLevelByName(name));
    }

    public void LoadLastScene()
    {
        StartCoroutine(LoadLevel(s_lastLoadedScene));
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
