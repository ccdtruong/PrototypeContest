using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]public void PlayGame()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
