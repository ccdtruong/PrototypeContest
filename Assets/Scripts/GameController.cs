using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Player m_grumpy;

    [SerializeField]
    private Player m_rebellious;

    private Vector3 m_screenBounds;
    private bool m_isGateOpened;
    private int m_playerPassed;
    private CameraZoom m_cameraZoom;

    [SerializeField] private Text heartLabel;
    private int heart;
    public int Heart
    {
        get { return heart; }
        set
        {
            heart = value;
            heartLabel.GetComponent<Text>().text = "x" + heart;
            if (heart == 0)
            {
                LoseGame();
            }
        }
    }

    [SerializeField] private Text coinLabel;
    private int coin;
    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            coinLabel.GetComponent<Text>().text = "x" + coin;
        }
    }

    private void Awake()
    {
        m_screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Start()
    {
        m_cameraZoom = GameObject.Find("CameraZoom").GetComponent<CameraZoom>();
        m_isGateOpened = false;
        if (GameSettings.m_mode == GameSettings.GameMode.Single)
            m_grumpy.SetSelected(true);
        m_playerPassed = 0;
        Coin = 0;
        Heart = 1;
        Invoke("PlayBackgroundMusic", .5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameSettings.m_mode == GameSettings.GameMode.Duo) ConnectionController.Update();
        if (CrossPlatformInputManager.GetButtonDown("Switch"))
        {
            SwitchCharacter();
        }
        if (heart <= 0)
        {
            LoseGame();
        }
        if (m_cameraZoom.GetCameraBehavior() == CameraBehavior.Idle)
        {
            m_screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        }
        Debug.Log(m_screenBounds);
    }

    private void FixedUpdate()
    {
    }

    public void PlayBackgroundMusic()
    {
        //if (!SoundManager.IsPlaying("background"))
        {
            SoundManager.Instance.Play("background");
        }
    }

    public Vector3 GetBounds()
    {
        return m_screenBounds;
    }

    private void SwitchCharacter()
    {
        if (GameSettings.m_mode == GameSettings.GameMode.Duo)
            return;
        if (m_grumpy.IsSelected())
        {
            m_rebellious.SetSelected(true);
            m_grumpy.SetSelected(false);
        }
        else
        {
            m_rebellious.SetSelected(false);
            m_grumpy.SetSelected(true);
        }
    }

    public void OpenGate()
    {
        m_isGateOpened = true;
        //Camera zoom in

        m_cameraZoom.ZoomIn();

        StartCoroutine(TransformGateState(2f, CameraBehavior.ZoomIn));
        StartCoroutine(TransformGateState(2.5f, CameraBehavior.ZoomOut));

    }

    IEnumerator TransformGateState(float sec, CameraBehavior cbh)
    {
        if (cbh == CameraBehavior.ZoomIn)
        {
            yield return new WaitForSeconds(sec);
            //Change Gate's spriteRenderer
            GateScript gateObject = GameObject.Find("Gate").GetComponent<GateScript>();
            gateObject.OpenGate();
        }
        else if (cbh == CameraBehavior.ZoomOut)
        {
            yield return new WaitForSeconds(sec);
            m_cameraZoom.ZoomOut();
        }
    }

    public void SetGateState(bool state)
    {
        m_isGateOpened = state;
    }

    public bool IsGateOpened()
    {
        return m_isGateOpened;
    }

    public void WinGame()
    {
        Debug.Log("WIN WIN WIN");
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel();
        //GameObject.Find("LevelLoader").GetComponent<LevelLoader>().Reload();
    }

    public void PlayerPassTheGate(GameObject go)
    {
        Debug.Log(go.name);
        if (m_isGateOpened)
        {
            go.SetActive(false);
            m_playerPassed++;
        }
        if (m_playerPassed == 2)
        {
            WinGame();
        }
    }

    public void LoseGame()
    {
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadSceneByName("Lost");
    }
}
